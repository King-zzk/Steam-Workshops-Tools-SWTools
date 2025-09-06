using System;
using System.Text;
using System.Text.Json;
using Serilog;
using static SWTools.Core.Item;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品物品列表 (容器)
    /// </summary>
    public class ItemList : List<Item> {
        // 空列表
        public static readonly ItemList Empty = [];

        // 检查是否包含指定 ItemId 的物品
        public bool Contains(string itemId) {
            foreach (var item in this) {
                if (item.ItemId.Equals(itemId)) {
                    return true;
                }
            }
            return false;
        }

        // 查找指定 Item ID 对应的物品
        public Item Find(string itemId) {
            foreach (var item in this) {
                if (item.ItemId == itemId) {
                    return item;
                }
            }
            return Item.Empty;
        }

        // 查找指定 Item ID 对应的物品，返回索引
        public int FindIndex(string itemId) {
            for (var i = 0; i < Count; i++) {
                if (this[i].ItemId == itemId) {
                    return i;
                }
            }
            return -1;
        }

        // 序列化到 Json
        public override string ToString() {
            try {
                return JsonSerializer.Serialize(this, Helper._jsonOptions);
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when serializing Json:\n{Exception}", ex);
                return string.Empty;
            }
        }

        // 保存到 Json
        public bool Save(in string fileName) {
            try {
                using StreamWriter sw = new(fileName);
                sw.Write(ToString());
                Log.Information("Succeessfully save {Count} item(s) to {Filaname}", Count, fileName);
                return true;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when saving {FileName}:\n{Exception}",
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
                var list = JsonSerializer.Deserialize<ItemList>(jsonString, Helper._jsonOptions);
                if (list == null) return Empty;
                list.RemoveEmptyItem();
                Log.Information("Load {Count} item(s) from {Filaname}", list.Count, fileName);
                return list;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when loading from {FileName}:\n{Exception}",
                    fileName, ex);
                return Empty;
            }
        }

        // 用传入物品列表更新当前列表
        public void UpdateWith(in ItemList another) {
            for (var i = 0; i < Count; i++) {
                if (another.Contains(this[i].ItemId)) {
                    this[i] = another.Find(this[i].ItemId);
                }
            }
        }

        // 解析全部队列中物品
        public void ParseAll() {
            List<string> items = new();
            StringBuilder sb = new();
            sb.Append('[');
            foreach (var item in this) {
                if (item.ParseState == Item.EParseState.InQueue) {
                    items.Add(item.ItemId);
                    if (sb.Length > 1) sb.Append(',');
                    sb.Append(item.ItemId);
                }
            }
            sb.Append(']');
            string str = Helper.MakeHttpPost(SwdApi.Url, sb.ToString());
            // 处理回复
            try {
                var response = JsonSerializer.Deserialize<SwdApi.Response[]>(str, Helper._jsonOptions);
                if (response == null) throw new Exception("response is null");
                for (var i = 0; i < response.Length; i++) {
                    if (!Contains(items[i])) continue;
                    this[FindIndex(items[i])].ParseWith(response[i]);
                }
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when deserializing Json:\n{Exception}", ex);
            }
        }

        // 移除空物品
        private void RemoveEmptyItem() {
            List<Item> itemsToRemove = [];
            foreach (var item in this) {
                if (item.ItemId == "") itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove) {
                Remove(item);
            }
        }
    }
}
