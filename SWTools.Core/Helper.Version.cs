using System;
using System.Reflection;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (版本与更新)
    /// </summary>
    public partial class Helper {
        private static readonly AssemblyName _assemblyName = Assembly.GetExecutingAssembly().GetName();
        private const string _versionUrl = "https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt";

        public static readonly Version EmptyVersion = new();

        // 获取项目版本信息
        public static Version GetVersion() {
            if (_assemblyName.Version == null) {
                return EmptyVersion;
            }
            return _assemblyName.Version;
        }

        // 获取最新版本
        public static Version RequestLatestVersion() {
            string str;
            foreach (var proxy in _githubProxies) {
                str = Helper.MakeHttpGet(proxy + _versionUrl);
                if (str != string.Empty) {
                    if (Version.TryParse(str, out Version? version)) {
                        Log.Logger.Debug("Received version info from \"{Url}\" (version {Version})",
                            proxy + _versionUrl, version);
                        return version;
                    } else {
                        continue;
                    }
                }
            }
            return EmptyVersion;
        }
    }
}
