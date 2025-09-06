using Serilog;
using System;
using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// 配置管理器 (静态类)
    /// </summary>
    public static class ConfigManager {
        // 配置文件名
        private const string _fileName = "Config.json";

        // 配置
        public static Config Config { get; set; } = new();

        // 保存到 Json
        public static bool Save() {
            try {
                using StreamWriter sw = new(_fileName);
                sw.Write(Config.ToString());
                Log.Information("Succeessfully save Config to {Filaname}", _fileName);
                return true;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when saving {FileName}:\n{Exception}",
                    _fileName, ex);
                return false;
            }
        }

        // 从 Json 读取
        public static void Load() {
            if (!File.Exists(_fileName)) {
                Log.Logger.Error("{Filename} not found, skipping loading:",
                    _fileName);
            }
            try {
                string jsonString;
                using StreamReader sr = new(_fileName);
                jsonString = sr.ReadToEnd();
                var config = JsonSerializer.Deserialize<Config>(jsonString, Helper._jsonOptions);
                if (config == null) throw new Exception("config is null");
                Config = config;
                Log.Information("Loaded config from {Filaname}", _fileName);
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when loading {Filename}:\n{Exception}",
                    _fileName, ex);
            }
        }
    }
}
