using System;
using System.Diagnostics;
using System.Text;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品 (核心逻辑)
    /// </summary>
    public partial class Item {
        // 解析 API.SwDownloader 返回的数据
        public void ParseWith(in API.SwDownloader.Response swdResponse) {
            lock (this) {
                ItemTitle = swdResponse.title;
                ItemSize = long.Parse(swdResponse.file_size);
                AppId = swdResponse.consumer_appid;
                AppName = swdResponse.app_name;
                if (string.IsNullOrEmpty(swdResponse.filename)) {
                    IsFree = true;
                    UrlFreeDownload = swdResponse.filename;
                } else {
                    IsFree = false;
                    UrlFreeDownload = string.Empty;
                }
                UrlPreview = swdResponse.preview_url;
                // 设置状态
                if (AppId == 0) {
                    ParseState = EParseState.Failed;
                    LogManager.Log.Error("Failed to parse item {ItemId}, AppId is 0", ItemId);
                } else {
                    ParseState = EParseState.Done;
                    LogManager.Log.Debug("Parsed item {ItemId}", ItemId);
                    // 保存到缓存
                    Cache.Parse.Store(this);
                }
            }
        }

        // 下载总逻辑
        public async Task<bool> Download() {
            if (IsFree) {
                if(await Helper.DownloadFile(UrlFreeDownload, GetDownloadPath())) {
                    return true;
                } else {
                    LogManager.Log.Warning("Failed to download {ItemId} with UrlFreeDownload, " +
                        "tring download with steamcmd", ItemId);
                }
            }
            var accounts = AccountManager.GetAccountFor(AppId);
            lock (this) {
                foreach (var account in accounts) {
                    if (DownloadWithSteamcmd(account)) {
                        return true;
                    } else {
                        if (FailReason != EFailReason.AccountDisabled &&
                            FailReason != EFailReason.InvalidPassword) {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        // 用 Steamcmd 下载
        /// <exception cref="FileNotFoundException"></exception>
        private bool DownloadWithSteamcmd(in Account account) {
            // 检查 Steamcmd 是否存在
            string steamcmdPath = ConfigManager.Config.SteamcmdPath + "steamcmd.exe";
            if (!File.Exists(steamcmdPath)) {
                LogManager.Log.Error("\"{SteamCmdPath}\" not found", steamcmdPath);
                throw new FileNotFoundException($"\"{steamcmdPath}\" not found");
            }
            // 启动 Steamcmd
            DownloadState = EDownloadState.Handling;
            FailReason = EFailReason.Null;
            _exceptionMsg = string.Empty;
            ProcessStartInfo startInfo = Helper.GetProcessStartInfo(
                    $"+login {account.Name} {account.Password} " +
                    $"+workshop_download_item {AppId} {ItemId} " +
                    $"+quit"
                );
            try {
                using Process? process = Process.Start(startInfo) ?? throw new Exception("process is null");
                StringBuilder downloadLog = new();
                process.OutputDataReceived += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        downloadLog.AppendLine(e.Data);
                        LogManager.Log.Debug("Steamcmd: {Message}", e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                if (!Directory.Exists(GetDownloadPath())) { // 下载失败了
                    DownloadState = EDownloadState.Failed;
                    GetFailReason(downloadLog.ToString());
                    return false;
                }
                DownloadState = EDownloadState.Done;
                LogManager.Log.Information("Downloaded item {ItemId} to \"{Path}\"",
                    ItemId, GetDownloadPath());
            } catch (Exception ex) {
                LogManager.Log.Error("Exception occured when downloading {ItemId}:\n{Exception}", ItemId, ex);
                DownloadState = EDownloadState.Failed;
                FailReason = EFailReason.Exception;
                _exceptionMsg = ex.Message;
            }
            return true;
        }
    }
}