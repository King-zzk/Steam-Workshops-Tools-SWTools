using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWTools.Core.Cache {
    /// <summary>
    /// 全局解析缓存
    /// </summary>
    internal static class Parse {
        // 缓存
        public static ItemList Cache { get; set; } = [];

        // 查找缓存
        public static Item? Get(string itemId) {
            return Cache.Find(itemId);
        }
        // 存入缓存 (暂存)
        public static void Store(Item item) {
            lock (Cache) {
                if (Cache.Contains(item.ItemId)) {
                    Cache.Remove(Cache.Find(item.ItemId));
                }
                Cache.Add(item);
            }
        }

        // 加载缓存
        public static void Load() {
            if (ConfigManager.Config.NoCache) return;
            lock (Cache) {
                if (!File.Exists(Constants.CacheParseFile)) return;
                var cache = ItemList.Load(Constants.CacheParseFile);
                if (cache != null) {
                    Cache = cache;
                }
            }
        }
        // 保存缓存
        public static void Save() {
            lock (Cache) {
                var path = Path.GetDirectoryName(Constants.CacheParseFile);
                if (path == null) return;
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                Cache.Save(Constants.CacheParseFile);
            }
        }
        // 清空缓存
        public static void Clear() {
            lock (Cache) {
                Cache.Clear();
                if (File.Exists(Constants.CacheParseFile)) {
                    File.Delete(Constants.CacheParseFile);
                    LogManager.Log.Information("Deleted {Filaname}", Constants.CacheParseFile);
                }
                LogManager.Log.Information("Cleared all parse cache");
            }
        }
    }
}
