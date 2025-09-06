using System;
using Serilog;
using Serilog.Enrichers.CallerInfo;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (日志器)
    /// </summary>
    public static partial class Helper {
        private const string _logFileName = "latest.log";
        private const string _logOutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] {Namespace}.{Method}: {Message:lj}{NewLine}{Exception}";

        // 配置并启动日志器
        public static void SetupLogger() {
            // 删除旧日志
            if (File.Exists(_logFileName)) {
                File.Delete(_logFileName);
            }
            // 配置日志器
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithCallerInfo(includeFileInfo: false,
                    assemblyPrefix: "SWTools.")
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: _logOutputTemplate)
                .WriteTo.File(_logFileName,
                    outputTemplate: _logOutputTemplate,
                    rollingInterval: RollingInterval.Infinite)
                .CreateLogger();
            // 启动消息
            Log.Information("*** This is SWTools vesion {Version} ***", GetVersionStr());
        }
    }
}
