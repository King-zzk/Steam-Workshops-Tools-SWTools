using Serilog;
using Serilog.Enrichers.CallerInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SWTools.Core {
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LogManager {
        // 访问点
        public static ILogger Log { get; private set; }

        // 日志字符串
        public static StringWriter LogWriter { get; private set; } = new();

        // 启动
        public static void Setup() {
            // 删除旧日志
            bool failed = false;
            if (File.Exists(Constants.LogFile)) {
                try {
                    File.Delete(Constants.LogFile);
                }
                catch (Exception) {
                    failed = true;
                }
            }
            if (!Directory.Exists(Constants.LogDir)) {
                Directory.CreateDirectory(Constants.LogDir);
            }
            // 配置日志器
            if (ConfigManager.Config.LogDebug) {
                Log = new LoggerConfiguration()
                    .Enrich.WithCallerInfo(includeFileInfo: false,
                        assemblyPrefix: "SWTools.")
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: Constants.LogTemplateDebug)
                    .WriteTo.File(Constants.LogFile,
                        outputTemplate: Constants.LogTemplateDebug,
                        rollingInterval: RollingInterval.Infinite)
                    .WriteTo.TextWriter(LogWriter, outputTemplate: Constants.LogTemplateDebug)
                    .CreateLogger();
                Log.Debug("Log level has been set to DEBUG");
            } else {
                Log = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(outputTemplate: Constants.LogTemplate)
                    .WriteTo.File(Constants.LogFile,
                        outputTemplate: Constants.LogTemplate,
                        rollingInterval: RollingInterval.Infinite)
                    .WriteTo.TextWriter(LogWriter, outputTemplate: Constants.LogTemplate)
                    .CreateLogger();
            }
            // 炫酷的启动消息
            // https://www.lddgo.net/string/text-to-ascii-art 字体: ANSI Shadow
            Log.Information(@"

███████╗██╗    ██╗████████╗ ██████╗  ██████╗ ██╗     ███████╗   SWTools.Core v{Version}
██╔════╝██║    ██║╚══██╔══╝██╔═══██╗██╔═══██╗██║     ██╔════╝   
███████╗██║ █╗ ██║   ██║   ██║   ██║██║   ██║██║     ███████╗   Copyright (c) 2025 Commkom (king-zzk), 
╚════██║██║███╗██║   ██║   ██║   ██║██║   ██║██║     ╚════██║   masterLazy (mLazy).
███████║╚███╔███╔╝   ██║   ╚██████╔╝╚██████╔╝███████╗███████║
╚══════╝ ╚══╝╚══╝    ╚═╝    ╚═════╝  ╚═════╝ ╚══════╝╚══════╝   Licensed under GPL-2.0.
", Constants.Version);
            // 错误
            if (failed) {
                Log.Error("Failed to delete {LogFile}", Constants.LogFile);
                Log.Warning("Is there another instance running? This may make the application UNSTABLE!");
            }
        }
    }
}
