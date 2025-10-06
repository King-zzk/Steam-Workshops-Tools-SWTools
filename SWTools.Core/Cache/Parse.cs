namespace SWTools.Core.Cache {
    /// <summary>
    /// 全局解析缓存
    /// </summary>
    internal static class Parse {
        // 缓存
        private static ItemList _cache = [];

        // 查找缓存
        public static Item? Get(string itemId) {
            return _cache.Find(itemId);
        }
        // 存入缓存 (暂存)
        public static void Store(in Item item) {
            lock (_cache) {
                if (_cache.Contains(item.ItemId)) {
                    _cache.Remove(_cache.Find(item.ItemId)!);
                }
                item.DownloadState = Item.EDownloadState.Pending;
                _cache.Add(item);
            }
        }

        // 加载缓存
        public static void Load() {
            if (ConfigManager.Config.NoCache) return;
            lock (_cache) {
                if (!File.Exists(Constants.CacheParseFile)) return;
                var cache = ItemList.Load(Constants.CacheParseFile);
                if (cache != null) {
                    foreach (var item in cache) {
                        if (item.IsCompleted()) {
                            item.DownloadState = Item.EDownloadState.Pending;
                            _cache.Add(item);
                        }
                    }
                }
            }
        }
        // 保存缓存
        public static void Save() {
            lock (_cache) {
                var path = Path.GetDirectoryName(Constants.CacheParseFile);
                if (path == null) return;
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                _cache.Save(Constants.CacheParseFile);
            }
        }
        // 清空缓存
        public static void Clear() {
            lock (_cache) {
                _cache.Clear();
                if (File.Exists(Constants.CacheParseFile)) {
                    File.Delete(Constants.CacheParseFile);
                    LogManager.Log.Information("Deleted {Filaname}", Constants.CacheParseFile);
                }
                LogManager.Log.Information("Cleared all parse cache");
            }
        }
    }
}
