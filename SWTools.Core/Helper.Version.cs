using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (版本与更新)
    /// </summary>
    public static partial class Helper {
        private static readonly AssemblyName _assemblyName = Assembly.GetExecutingAssembly().GetName();

        public static readonly Version EmptyVersion = new();

        // 获取项目版本信息
        public static Version Version {
            get {
            if (_assemblyName.Version == null) {
                return EmptyVersion;
            }
            return _assemblyName.Version;
            }
        }

        // 获取项目版本信息字符串
        public static string VersionStr {
            get {
                return Version.ToString(3);
            }
        }

        // 获取最新版本
        public async static Task<Version> RequestLatestVersion() {
            string str;
            foreach (var proxy in Constants.UrlGithubProxy) {
                str = await Helper.MakeHttpGet(proxy + Constants.UrlVersion);
                if (str != string.Empty) {
                    if (Version.TryParse(str, out Version? version)) {
                        LogManager.Log.Debug("Received version info from \"{Url}\" (version {Version})",
                            proxy + Constants.UrlVersion, version);
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
