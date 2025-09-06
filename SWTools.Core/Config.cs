using Serilog;
using System;
using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// 可自定义的配置
    /// </summary>
    public class Config {
        // 空配置
        public static readonly Config Empty = new();

        // 配置
#if DEBUG
        public bool LogDebug { get; set; } = true;
#else
        public bool LogDebug { get; set; } = false;
#endif
        public string SteamcmdPath { get; set; } = "Steamcmd\\";

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
    }
}
