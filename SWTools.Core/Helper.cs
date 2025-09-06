using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.IO.Compression;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static partial class Helper {
        private const string _steamcmdUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";

        // Json 选项
        public static readonly JsonSerializerOptions _jsonOptions = new() {
            // 序列化选项
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
            // 反序列化选项
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        };

        // Gitub 代理
        public static readonly string[] _githubProxies = [
            "", // 这个空的不能删
            "https://gh.llkk.cc/",
            "https://gitproxy.click/",
            "https://ghproxy.net/"
            // 3 个够了
        ];

        // 安装 Steamcmd
        public static bool SetupSteamcmd() {
            // 安装目录存在
            if (Directory.Exists(ConfigManager.Config.SteamcmdPath)) {
                Log.Logger.Warning("Directory \"{StramcmdPath}\" already exists, skipping download",
                    ConfigManager.Config.SteamcmdPath);
            } else { // 下载
                Directory.CreateDirectory(ConfigManager.Config.SteamcmdPath);
                if (!DownloadFile(_steamcmdUrl, ConfigManager.Config.SteamcmdPath + "temp.zip")) {
                    Log.Logger.Error("Failed to download");
                    return false;
                }
                try {
                    // 解压，删包
                    ZipFile.ExtractToDirectory(ConfigManager.Config.SteamcmdPath + "temp.zip", ConfigManager.Config.SteamcmdPath);
                    File.Delete(ConfigManager.Config.SteamcmdPath + "temp.zip");
                    // 测试
                    if (!File.Exists(ConfigManager.Config.SteamcmdPath + "steamcmd.exe")) {
                        Log.Error("Failed to access steamcmd.exe, unknown reason", ConfigManager.Config.SteamcmdPath);
                        return false;
                    }
                    Log.Information("Installed steamcmd at \"{SteamcmdPath}\"", ConfigManager.Config.SteamcmdPath);
                }
                catch (Exception ex) {
                    Log.Logger.Error("Exception occured:\n{Exception}", ex);
                    return false;
                }
            }
            // 启动并完成更新
            ProcessStartInfo startInfo = GetProcessStartInfo("+quit");
            try {
                using Process? process = Process.Start(startInfo) ?? throw new Exception("process is null");
                process.OutputDataReceived += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Log.Logger.Debug("Steamcmd: {Message}", e.Data);
                    }
                };
                process.Start();
                if (ConfigManager.Config.LogDebug) {
                    process.BeginOutputReadLine();
                }
                process.WaitForExit();
                Log.Logger.Information("Completed update of steamcmd");
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured:\n{Exception}", ex);
                return false;
            }
            return true;
        }

        // 获取 Steamcmd 启动信息
        public static ProcessStartInfo GetProcessStartInfo(in string arguments) {
            return new() {
                FileName = ConfigManager.Config.SteamcmdPath + "steamcmd.exe",
                Arguments = arguments,
                WorkingDirectory = ConfigManager.Config.SteamcmdPath,
                UseShellExecute = false,
                CreateNoWindow = true, // 不能省略
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
                StandardInputEncoding = Encoding.UTF8,
            };
        }
    }
}
