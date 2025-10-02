using System;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;
using Serilog;

namespace SWTools.Core.Helper {
    /// <summary>
    /// 辅助方法 (Steamcmd)
    /// </summary>
    public static class Steamcmd {
        // 安装 Steamcmd
        public static async Task<bool> Setup() {
            // 安装目录存在
            if (Directory.Exists(Constants.SteamcmdDir)) {
                LogManager.Log.Warning("Directory \"{StramcmdPath}\" already exists, skipping download",
                    Constants.SteamcmdDir);
            } else { // 下载
                Directory.CreateDirectory(Constants.SteamcmdDir);
                if (!await Http.DownloadFile(Constants.UrlSteamcmd, Constants.SteamcmdDir + "temp.zip")) {
                    LogManager.Log.Error("Failed to download");
                    return false;
                }
                try {
                    // 解压，删包
                    await ZipFile.ExtractToDirectoryAsync(Constants.SteamcmdDir + "temp.zip", Constants.SteamcmdDir);
                    File.Delete(Constants.SteamcmdDir + "temp.zip");
                    // 测试
                    if (!File.Exists(Constants.SteamcmdFile)) {
                        Log.Error("Failed to access steamcmd.exe, unknown reason", Constants.SteamcmdDir);
                        return false;
                    }
                    Log.Information("Installed steamcmd at \"{SteamcmdPath}\"", Constants.SteamcmdDir);
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
                        LogManager.Log.Information("Steamcmd: {Message}", e.Data);
                    }
                };
                process.Start();
                LogManager.Log.Information("Steamcmd has been launched, waiting for its update");
                process.BeginOutputReadLine();
                await process.WaitForExitAsync();
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
                FileName = Constants.SteamcmdFile,
                Arguments = arguments,
                WorkingDirectory = Constants.SteamcmdDir,
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
