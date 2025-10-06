using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// 配置管理器 (静态类)
    /// </summary>
    public static class ConfigManager {
        // 配置
        public static Config Config { get; set; } = new();

        // 初始化
        public static void Setup() {
            Load();
            Config.PropertyChanged += (s, e) => {
                Save("Autosave");
            };
        }

        // 保存到 Json
        public static bool Save(string? reason = null) {
            if (!Directory.Exists(Constants.CommonDir)) {
                Directory.CreateDirectory(Constants.CommonDir);
            }
            try {
                using StreamWriter sw = new(Constants.ConfigFile);
                sw.Write(Config.ToString());
                if (reason == null) {
                    LogManager.Log.Information("Saved save config to {Filaname}", Constants.ConfigFile);
                } else {
                    LogManager.Log.Information("Saved config to {Filaname} ({Reason})", 
                        Constants.ConfigFile, reason);
                }
                return true;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when saving {FileName}:\n{Exception}",
                    Constants.ConfigFile, ex);
                return false;
            }
        }

        // 从 Json 读取
        public static void Load() {
            // 此方法内必须用 LogManager.Log?. !!!
            if (!File.Exists(Constants.ConfigFile)) {
                LogManager.Log?.Warning("{Filename} not found, skipping loading",
                    Constants.ConfigFile);
                return;
            }
            try {
                string jsonString;
                using StreamReader sr = new(Constants.ConfigFile);
                jsonString = sr.ReadToEnd();
                var config = JsonSerializer.Deserialize<Config>(jsonString, Constants.JsonOptions);
                if (config == null)
                    throw new Exception("config is null");
                Config = config;
                LogManager.Log?.Information("Loaded config from {Filaname}", Constants.ConfigFile);
            }
            catch (Exception ex) {
                LogManager.Log?.Error("Exception occured when loading {Filename}:\n{Exception}",
                    Constants.ConfigFile, ex);
            }
        }
    }
}
