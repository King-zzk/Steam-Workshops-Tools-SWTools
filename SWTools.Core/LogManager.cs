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
        // 更新事件
        public static event EventHandler<string> LogMessageAdded;

        // 启动
        public static void Setup() {
            // 删除旧日志
            if (File.Exists(Constants.LogFileName)) {
                File.Delete(Constants.LogFileName);
            }
            if (!Directory.Exists(Constants.LogDirName)) {
                Directory.CreateDirectory(Constants.LogDirName);
            }
            // 配置日志器
            if (ConfigManager.Config.LogDebug) {
                Log = new LoggerConfiguration()
                    .Enrich.WithCallerInfo(includeFileInfo: false,
                        assemblyPrefix: "SWTools.")
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: Constants.LogTemplateDebug)
                    .WriteTo.File(Constants.LogFileName,
                        outputTemplate: Constants.LogTemplateDebug,
                        rollingInterval: RollingInterval.Infinite)
                    .WriteTo.TextWriter(LogWriter, outputTemplate: Constants.LogTemplateDebug)
                    .CreateLogger();
                Log.Debug("Log level has been set to DEBUG");
            } else {
                Log = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(outputTemplate: Constants.LogTemplate)
                    .WriteTo.File(Constants.LogFileName,
                        outputTemplate: Constants.LogTemplateDebug,
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
", Helper.VersionStr);
        }
    }
}
