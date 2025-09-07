using System;
using Serilog;
using Serilog.Enrichers.CallerInfo;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (日志器)
    /// </summary>
    public static partial class Helper {
        private const string _logFileName = "latest.log";
        private const string _logTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        private const string _logTemplateDebug =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Namespace}.{Method}: {Message:lj}{NewLine}{Exception}";

        // 配置并启动日志器
        public static void SetupLogger() {
            // 删除旧日志
            if (File.Exists(_logFileName)) {
                File.Delete(_logFileName);
            }
            // 配置日志器
            if (ConfigManager.Config.LogDebug) {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.WithCallerInfo(includeFileInfo: false,
                        assemblyPrefix: "SWTools.")
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: _logTemplateDebug)
                    .WriteTo.File(_logFileName,
                        outputTemplate: _logTemplateDebug,
                        rollingInterval: RollingInterval.Infinite)
                    .CreateLogger();
                Log.Logger.Debug("Log level has been set to DEBUG");
            } else {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(outputTemplate: _logTemplate)
                    .WriteTo.File(_logFileName,
                        outputTemplate: _logTemplateDebug,
                        rollingInterval: RollingInterval.Infinite)
                    .CreateLogger();
            }
            // 启动消息
            Log.Information("*** This is SWTools.Core v{Version} ***", GetVersionStr());
        }
    }
}
