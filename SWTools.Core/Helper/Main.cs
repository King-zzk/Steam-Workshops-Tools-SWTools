using System.Text.Json;

namespace SWTools.Core.Helper {
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static class Main {
        // 启动组件
        public static void SetupAll() {
            // 必须先加载这俩 (这之前不要使用日志器)
            ConfigManager.Setup();
            LogManager.Setup();

            // 加载其他组件
            Cache.Parse.Load();
            AccountManager.Setup();

            LogManager.Log.Information("Completed setting up Core");
        }

        // 关闭组件
        public static void CleanupAll() {
            ConfigManager.Save("Exit");
            if (!ConfigManager.Config.NoCache) {
                Cache.Parse.Save();
            }
        }

        // 清空所有缓存
        public static void ClearAllCache() {
            try {
                Cache.Parse.Clear();
                Directory.Delete(Constants.PreviewDir, true);
                LogManager.Log.Information("Deleted {CacheDir}", Constants.PreviewDir);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when deleting {CacheDir}:\n{Exception}", Constants.CacheDir, ex);
            }
        }

        // 忽略后缀名地查找文件 (返回文件名)
        public static string? FindFileIgnoreExt(string path, string fileNameWithoutExt) {
            if (!Directory.Exists(path)) return null;
            string[] allFiles = Directory.GetFiles(path);
            foreach (string file in allFiles) {
                string name = Path.GetFileName(file);
                string nameNoExt = Path.GetFileNameWithoutExtension(file);
                if (nameNoExt.Equals(
                    fileNameWithoutExt,
                    StringComparison.OrdinalIgnoreCase)) {
                    return name;
                }
            }
            return null;
        }

        // 读取本地 “最新信息” 文件
        public static API.LatestInfo.Response? ReadLatestInfo() {
            try {
                string jsonString;
                using StreamReader sr = new(Constants.LatestInfoFile);
                jsonString = sr.ReadToEnd();
                return JsonSerializer.Deserialize<API.LatestInfo.Response>(jsonString, Constants.JsonOptions);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when loading from {FileName}:\n{Exception}", Constants.LatestInfoFile, ex);
                return null;
            }
        }
    }
}
