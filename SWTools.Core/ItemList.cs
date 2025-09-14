using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using PropertyChanged;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品物品列表 (容器)
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class ItemList : ObservableCollection<Item> {
        public bool IsDownloading { get; set; } = false;        // 是否自动启动下载
        public int Downloading { get; set; } = 0;      // 正在下载的物品个数

        // 检查是否包含指定 ItemId 的物品
        public bool Contains(in string itemId) {
            foreach (var item in this) {
                if (item.ItemId.Equals(itemId)) {
                    return true;
                }
            }
            return false;
        }

        // 查找指定 Item ID 对应的物品
        public Item? Find(in string itemId) {
            foreach (var item in this) {
                if (item.ItemId == itemId) {
                    return item;
                }
            }
            return null;
        }

        // 查找指定 Item ID 对应的物品，返回索引
        public int FindIndex(in string itemId) {
            for (var i = 0; i < Count; i++) {
                if (this[i].ItemId == itemId) {
                    return i;
                }
            }
            return -1;
        }

        // 添加物品
        public new void Add(Item item) {
            if (!Contains(item.ItemId)) {
                base.Add(item);
                if (IsDownloading) { // 自动开始下载
                    _ = DownloadOne(item.ItemId);
                }
            }
        }

        // 序列化到 Json
        public override string ToString() {
            try {
                return JsonSerializer.Serialize(this, Constants.JsonOptions);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when serializing Json:\n{Exception}", ex);
                return string.Empty;
            }
        }

        // 保存到 Json
        public bool Save(in string fileName) {
            try {
                using StreamWriter sw = new(fileName);
                sw.Write(ToString());
                Log.Information("Saved {Count} item(s) to {Filaname}", Count, fileName);
                return true;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when saving {FileName}:\n{Exception}",
                    fileName, ex);
                return false;
            }
        }

        // 从 Json 读取
        public static ItemList? Load(in string fileName) {
            try {
                string jsonString;
                using StreamReader sr = new(fileName);
                jsonString = sr.ReadToEnd();
                var list = JsonSerializer.Deserialize<ItemList>(jsonString, Constants.JsonOptions);
                if (list == null)
                    return null;
                list.RemoveEmptyItem();
                LogManager.Log.Information("Loaded {Count} item(s) from {Filaname}", list.Count, fileName);
                return list;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when loading from {FileName}:\n{Exception}",
                    fileName, ex);
                return null;
            }
        }

        // 解析全部队列中物品
        public async Task ParseAll() {
            if (!ConfigManager.Config.NoCache) await ParseWithCache();
            await ParseAllWithRequest();
        }

        // 缓存解析
        private async Task ParseWithCache() {
            // 设置状态
            foreach (var item in this) {
                if (item.ParseState == Item.EParseState.InQueue) {
                    item.ParseState = Item.EParseState.Handling;
                }
            }
            await Task.Delay(500); // 仪式感
            for (var i = 0; i < Count; i++) {
                if (this[i].ParseState == Item.EParseState.Handling &&
                    Cache.Parse.Get(this[i].ItemId) != null) {
                    this[i] = Cache.Parse.Get(this[i].ItemId);
                }
            }
            // 复位状态
            foreach (var item in this) {
                if (item.ParseState == Item.EParseState.Handling) {
                    item.ParseState = Item.EParseState.InQueue;
                }
            }
        }

        // 联网解析
        private async Task ParseAllWithRequest() {
            // 请求 API
            List<string> items = [];
            foreach (var item in this) {
                if (item.ParseState == Item.EParseState.InQueue) {
                    item.ParseState = Item.EParseState.Handling;
                    items.Add(item.ItemId);
                }
            }
            if (items.Count == 0) return;
            var response = await API.SwDownloader.Request(items);
            // 处理回复，注意回复的序列可能和请求的不一致
            if (response == null) {
                LogManager.Log.Error("Failed to parse items");
            } else {
                for (var i = 0; i < response.Length; i++) {
                    if (!Contains(response[i].publishedfileid))
                        continue;
                    this[FindIndex(response[i].publishedfileid)].ParseWith(response[i]);
                }
            }
            // 设置状态
            foreach (var item in this) {
                if (items.Contains(item.ItemId) && item.ParseState == Item.EParseState.Handling) {
                    item.ParseState = Item.EParseState.Failed;
                }
            }
        }

        // 下载一项物品
        public async Task DownloadOne(string itemId) {
            if (Contains(itemId) &&
                this[FindIndex(itemId)].DownloadState == Item.EDownloadState.InQueue) {
                lock (this) { Downloading++; }
                await this[FindIndex(itemId)].Download();
                lock (this) { Downloading--; }
            }
        }

        // 移除空物品
        private void RemoveEmptyItem() {
            List<Item> itemsToRemove = [];
            foreach (var item in this) {
                if (string.IsNullOrEmpty(item.ItemId))
                    itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove) {
                Remove(item);
            }
        }

        // 检查已下载的物品是否丢失
        public void CheckDownloadedItems() {
            int count = 0;
            for (var i = 0; i < Count; i++) {
                if (this[i].DownloadState == Item.EDownloadState.Done &&
                    !Directory.Exists(this[i].GetDownloadPath())) {
                    this[i].DownloadState = Item.EDownloadState.Missing;
                    count++;
                }
            }
            LogManager.Log.Information("Found {Count} item(s) missing", count);
        }
    }
}
