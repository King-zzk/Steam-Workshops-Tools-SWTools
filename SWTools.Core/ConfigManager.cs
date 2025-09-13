using System;
using System.Text.Json;
using Serilog;

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
            if (!Directory.Exists(Constants.ConfigDirName)) {
                Directory.CreateDirectory(Constants.ConfigDirName);
            }
            try {
                using StreamWriter sw = new(Constants.ConfigFileName);
                sw.Write(Config.ToString());
                if (reason == null) {
                    LogManager.Log.Information("Succeessfully save Config to {Filaname}", Constants.ConfigFileName);
                } else {
                    LogManager.Log.Information("Succeessfully save Config to {Filaname} ({Reason})", 
                        Constants.ConfigFileName, reason);
                }
                return true;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when saving {FileName}:\n{Exception}",
                    Constants.ConfigFileName, ex);
                return false;
            }
        }

        // 从 Json 读取
        public static void Load() {
            if (!File.Exists(Constants.ConfigFileName)) {
                LogManager.Log?.Error("{Filename} not found, skipping loading:",
                    Constants.ConfigFileName);
                return;
            }
            try {
                string jsonString;
                using StreamReader sr = new(Constants.ConfigFileName);
                jsonString = sr.ReadToEnd();
                var config = JsonSerializer.Deserialize<Config>(jsonString, Constants.JsonOptions);
                if (config == null)
                    throw new Exception("config is null");
                Config = config;
                Log.Information("Loaded config from {Filaname}", Constants.ConfigFileName);
            }
            catch (Exception ex) {
                LogManager.Log?.Error("Exception occured when loading {Filename}:\n{Exception}",
                    Constants.ConfigFileName, ex);
            }
        }
    }
}
