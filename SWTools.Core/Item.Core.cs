using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品 (核心逻辑)
    /// </summary>
    public partial class Item {
        // 解析自己 (注意: 如果要解析多个物品，请使用 ItemList.ParseAll() )
        public void Parse() {
            ParseState = EParseState.Handling;
            string str = Helper.MakeHttpPost(SwdApi.Url, $"[{ItemId}]");
            if (str == "") {
                Log.Logger.Error("Failed to parse item {ItemId}: empty response", ItemId);
            }
            // 处理回复
            try {
                var response = JsonSerializer.Deserialize<SwdApi.Response[]>(str, Helper._jsonOptions);
                if (response == null) throw new Exception("response is null");
                ParseWith(response[0]);
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when deserializing Json:\n{Exception}", ex);
                ParseState = EParseState.Failed;
            }
        }

        // 解析 steamworkshopdownloader.io/api 返回的数据
        public void ParseWith(in SwdApi.Response swdResponse) {
            ItemTitle = swdResponse.title;
            ItemSize = long.Parse(swdResponse.file_size);
            AppId = swdResponse.consumer_appid;
            AppName = swdResponse.app_name;
            // 设置状态
            ParseState = EParseState.Done;
            Log.Debug("Parsed item {ItemId}", ItemId);
        }

        // 下载自己
        /// <exception cref="FileNotFoundException"></exception>
        public void Download(in Account account) {
            // 检查 Steamcmd 是否存在
            string steamcmdPath = ConfigManager.Config.SteamcmdPath + "steamcmd.exe";
            if (!File.Exists(steamcmdPath)) {
                Log.Logger.Error("\"{SteamCmdPath}\" not found", steamcmdPath);
                throw new FileNotFoundException($"\"{steamcmdPath}\" not found");
            }
            // 启动 Steamcmd
            DownloadState = EDownloadState.Handling;
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
                        Log.Logger.Debug("Steamcmd: {Message}", e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                if (!Directory.Exists(GetDownloadPath())) { // 下载失败了
                    DownloadState = EDownloadState.Failed;
                    GetFailReason(downloadLog.ToString());
                    return;
                }
                DownloadState = EDownloadState.Done;
                Log.Logger.Information("Downloaded item {ItemId} to \"{Path}\"",
                    ItemId, GetDownloadPath());
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when downloading {ItemId}:\n{Exception}", ItemId, ex);
                DownloadState = EDownloadState.Failed;
                FailReason = EFailReason.Exception;
                _exceptionMsg = ex.Message;
            }
        }
    }
}