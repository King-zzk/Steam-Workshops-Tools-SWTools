using System.Diagnostics;
using System.Text;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品 (核心逻辑)
    /// </summary>
    public partial class Item {
        // 解析 API.SwDownloader 返回的数据
        public void ParseWith(in API.SwDownloader.Response swdResponse) {
            lock (this) {
                ItemTitle = swdResponse.title ?? "";
                ItemSize = long.Parse(swdResponse.file_size ?? "");
                AppId = swdResponse.consumer_appid;
                AppName = swdResponse.app_name ?? "";
                // 免费下载
                if (!string.IsNullOrEmpty(swdResponse.file_url)) {
                    IsFree = true;
                    UrlFreeDownload = swdResponse.file_url;
                } else {
                    IsFree = false;
                    UrlFreeDownload = string.Empty;
                }
                UrlPreview = swdResponse.preview_url ?? "";
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
                if (!Directory.Exists(GetDownloadPath())) {
                    Directory.CreateDirectory(GetDownloadPath());
                }
                // 大部分都是截图
                var res = await Helper.Http.DownloadImage(UrlFreeDownload, GetDownloadPath() + ItemId) != null;
                if (!res) {
                    res = await Helper.Http.DownloadFile(UrlFreeDownload, GetDownloadPath() + ItemId);
                }
                if (res) {
                    DownloadState = EDownloadState.Done;
                    return true;
                } else {
                    LogManager.Log.Warning("Failed to download {ItemId} with UrlFreeDownload, " +
                        "tring download with steamcmd", ItemId);
                    Directory.Delete(GetDownloadPath(), true);
                }
            }
            var accounts = AccountManager.GetAccountFor(AppId);
            // 使用已有账户下载
            foreach (var account in accounts) {
                if (await DownloadWithSteamcmd(account)) {
                    return true;
                } else {
                    if (FailReason != EFailReason.AccountDisabled &&
                        FailReason != EFailReason.InvalidPassword) {
                        break;
                    }
                }
            }
            if (accounts.Count == 0) {
                // 尝试匿名下载
                return await DownloadWithSteamcmd(AccountManager.Anonymous);
            }
            return false;
        }

        // 用 Steamcmd 下载
        /// <exception cref="FileNotFoundException"></exception>
        private async Task<bool> DownloadWithSteamcmd(Account account) {
            // 检查 Steamcmd 是否存在
            if (!File.Exists(Constants.SteamcmdFile)) {
                LogManager.Log.Error("\"{SteamCmdPath}\" not found", Constants.SteamcmdFile);
                throw new FileNotFoundException($"\"{Constants.SteamcmdFile}\" not found");
            }
            // 启动 Steamcmd
            DownloadState = EDownloadState.Handling;
            FailReason = EFailReason.Null;
            _exceptionMsg = string.Empty;
            ProcessStartInfo startInfo = Helper.Steamcmd.GetProcessStartInfo(
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
                await process.WaitForExitAsync();
                // 检查文件是否存在
                if (!Directory.Exists(GetDownloadPath())) {
                    DownloadState = EDownloadState.Failed;
                    FailReason = GetFailReason(downloadLog.ToString());
                    if (FailReason == EFailReason.Unknown) {
                        LogManager.Log.Error("Download failed with unknown reason. Please check the log of Steamcmd");
                    } else {
                        LogManager.Log.Error("Download failed with reason {reason}", FailReason);
                    }
                    return false;
                }
                DownloadState = EDownloadState.Done;
                LogManager.Log.Information("Downloaded item {ItemId} to \"{Path}\"",
                    ItemId, GetDownloadPath());
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when downloading {ItemId}:\n{Exception}", ItemId, ex);
                DownloadState = EDownloadState.Failed;
                FailReason = EFailReason.Exception;
                _exceptionMsg = ex.Message;
            }
            return true;
        }
    }
}