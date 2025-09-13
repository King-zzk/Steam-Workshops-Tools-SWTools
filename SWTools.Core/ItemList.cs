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

        // 空列表
        public static readonly ItemList Empty = [];

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
        public Item Find(in string itemId) {
            foreach (var item in this) {
                if (item.ItemId == itemId) {
                    return item;
                }
            }
            return Item.Empty;
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
                    DownloadOne(item.ItemId);
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
        public static ItemList Load(in string fileName) {
            try {
                string jsonString;
                using StreamReader sr = new(fileName);
                jsonString = sr.ReadToEnd();
                var list = JsonSerializer.Deserialize<ItemList>(jsonString, Constants.JsonOptions);
                if (list == null)
                    return Empty;
                list.RemoveEmptyItem();
                LogManager.Log.Information("Loaded {Count} item(s) from {Filaname}", list.Count, fileName);
                return list;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when loading from {FileName}:\n{Exception}",
                    fileName, ex);
                return Empty;
            }
        }

        // 解析全部队列中物品
        public async Task ParseAll() {
            List<string> items = [];
            // 构造请求
            StringBuilder sb = new();
            sb.Append('[');
            foreach (var item in this) {
                if (item.ParseState == Item.EParseState.InQueue) {
                    item.ParseState = Item.EParseState.Handling; // 顺便设置状态
                    items.Add(item.ItemId);
                    if (sb.Length > 1)
                        sb.Append(',');
                    sb.Append(item.ItemId);
                }
            }
            sb.Append(']');
            string str = await Helper.MakeHttpPost(SwdApi.Url, sb.ToString());
            // 处理回复
            try {
                var response = JsonSerializer.Deserialize<SwdApi.Response[]>(str, Constants.JsonOptions);
                if (response == null)
                    throw new Exception("response is null");
                // 注意回复的序列可能和请求的不一致
                for (var i = 0; i < response.Length; i++) {
                    if (!Contains(response[i].publishedfileid))
                        continue;
                    this[FindIndex(response[i].publishedfileid)].ParseWith(response[i]);
                }
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when deserializing Json:\n{Exception}", ex);
                LogManager.Log.Debug("The request was: {Request}", sb.ToString());
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
