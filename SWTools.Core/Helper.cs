using System;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static partial class Helper {

        // 安装 Steamcmd
        public static async Task<bool> SetupSteamcmd() {
            // 安装目录存在
            if (Directory.Exists(ConfigManager.Config.SteamcmdPath)) {
                LogManager.Log.Warning("Directory \"{StramcmdPath}\" already exists, skipping download",
                    ConfigManager.Config.SteamcmdPath);
            } else { // 下载
                Directory.CreateDirectory(ConfigManager.Config.SteamcmdPath);
                if (!await DownloadFile(Constants.UrlSteamcmd, ConfigManager.Config.SteamcmdPath + "temp.zip")) {
                    LogManager.Log.Error("Failed to download");
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
                    LogManager.Log.Error("Exception occured when installing Steamcmd:\n{Exception}", ex);
                    return false;
                }
            }
            // 启动并完成更新
            ProcessStartInfo startInfo = GetProcessStartInfo("+quit");
            try {
                using Process? process = Process.Start(startInfo) ?? throw new Exception("process is null");
                process.OutputDataReceived += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        LogManager.Log.Debug("Steamcmd: {Message}", e.Data);
                    }
                };
                process.Start();
                if (ConfigManager.Config.LogDebug) {
                    process.BeginOutputReadLine();
                }
                process.WaitForExit();
                LogManager.Log.Information("Completed update of steamcmd");
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when launching Steamcmd:\n{Exception}", ex);
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

        // 忽略后缀名地查找文件 (返回文件名)
        public static string? FindFileIgnoreExt(string path, string fileNameWithoutExt) {
            if (!Directory.Exists(path)) return null;
            string[] allFiles = Directory.GetFiles(path);
            foreach (string file in allFiles) {
                string name = Path.GetFileName(file);
                string nameNoExt = Path.GetFileNameWithoutExtension(file);
                if (nameNoExt.Equals(
                    fileNameWithoutExt,
                    StringComparison.OrdinalIgnoreCase)) {
                    return name;
                }
            }
            return null;
        }
    }
}
