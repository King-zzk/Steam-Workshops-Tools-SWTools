using PropertyChanged;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class Item : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ItemId { get; set; } = string.Empty;      // 物品 Id
        public string ItemTitle { get; set; } = string.Empty;   // 物品标题
        public long ItemSize { get; set; }                      // 该物品的文件大小
        public long AppId { get; set; }                         // 物品属于的 App 的 Id
        public string AppName { get; set; } = string.Empty;     // 物品属于的 App 的名字
        public string UrlPreview { get; set; } = string.Empty;  // 预览文件地址
        // 有些物品不需要 Steamcmd 即可下载
        public bool IsFree { get; set; }                              // 是否不需要 Steamcmd
        public string UrlFreeDownload { get; set; } = string.Empty;   // 下载地址


        // 解析状态
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EParseState ParseState { get; set; } = EParseState.Pending;

        // 下载状态
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EDownloadState DownloadState { get; set; } = EDownloadState.Pending;

        // 下载失败原因
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EFailReason FailReason { get; set; } = EFailReason.Null;
        private string _exceptionMsg = string.Empty;

        // 解析之后的操作
        [JsonIgnore]
        public EAfterParse AfterParse { get; set; } = EAfterParse.Nothing;

        public enum EParseState {
            Pending, Handling, Done, Failed,
            Manual // 指示物品信息是用户自己指定的
        }
        public enum EDownloadState {
            Pending, Handling, Done, Failed, Missing
        }
        public enum EFailReason {
            Null, Unknown, Exception,
            FileNotFound, Timeout, NoConnection,
            AccountDisabled, InvalidPassword, NoMatch,
            AccessDenied, LockingFailed, AccountNoSupportThisDownload
        }
        public enum EAfterParse {
            Nothing, Download
        }

        public Item() { }
        public Item(string itemId) {
            ItemId = itemId;
        }

        // 序列化到 Json
        public override string ToString() {
            try {
                return JsonSerializer.Serialize(this, Constants.JsonOptions);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when serializing Json:\n{Exception}", ex);
                return string.Empty;
            }
        }

        // 获取失败原因对应的提示信息
        public string GetFailMessage() {
            switch (FailReason) {
                case EFailReason.Null:
                    return "无";
                case EFailReason.Exception:
                    return "异常：" + _exceptionMsg;
                case EFailReason.Unknown:
                    return "未知错误（查看日志获取更多信息）";
                case EFailReason.FileNotFound:
                    return "物品未找到，请检查物品 ID 是否正确";
                case EFailReason.Timeout:
                    return "下载超时，请重试";
                case EFailReason.NoConnection:
                    return "无网络连接";
                case EFailReason.AccountNoSupportThisDownload:
                    return "匿名账号不支持下载此游戏的创意工坊文件,可以反馈支持此游戏！";
                case EFailReason.AccountDisabled:
                    return "账号不可用，请尝试向开发者反映此问题";
                case EFailReason.InvalidPassword:
                    return "密码错误，请尝试向开发者反映此问题";
                case EFailReason.NoMatch:
                    return "Steam App 与物品 ID 不匹配";
                case EFailReason.AccessDenied:
                    return "账户权限不足";
                case EFailReason.LockingFailed:
                    return "文件被占用";
            }
            LogManager.Log.Error("Received unknown enum value: {FailReason}", FailReason);
            return string.Empty;
        }

        // 获取下载文件目录
        public string GetDownloadPath() {
            return Constants.SteamcmdDir + $"steamapps/workshop/content/{AppId}/{ItemId}/";
        }

        // 把状态二值化到 InQueue / Done
        public void FixState() {
            if (ParseState != EParseState.Done) {
                ParseState = EParseState.Pending;
            }
            if (DownloadState != EDownloadState.Done) {
                DownloadState = EDownloadState.Pending;
            }
        }

        // 解析下载日志，获取失败原因
        private static EFailReason GetFailReason(in string downloadLog) {
            if (downloadLog.Contains("File Not Found")) {
                return EFailReason.FileNotFound;
            } else if (downloadLog.Contains("Timeout")) {
                return EFailReason.Timeout;
            } else if (downloadLog.Contains("No Connection")) {
                return EFailReason.NoConnection;
            } else if (downloadLog.Contains("Account Disabled")) {
                return EFailReason.AccountDisabled;
            } else if (downloadLog.Contains("Invalid Password")) {
                return EFailReason.InvalidPassword;
            } else if (downloadLog.Contains("No match")) {
                return EFailReason.NoMatch;
            } else if (downloadLog.Contains("Failure")) {
                return EFailReason.AccountNoSupportThisDownload;
            } else if (downloadLog.Contains("Access Denied")) {
                return EFailReason.AccessDenied;
            } else if (downloadLog.Contains("Locking Failed")) {
                LogManager.Log.Warning("!!!!!!!!! You might meet a HEISENBUG\n!!!!!!!!! Please DO submit the following log to developers:\n" +
                    "{log}\nThis log will also be saved in HEISENBUG.log.",
                    downloadLog);
                using StreamWriter sw = new("HEISENBUG.log");
                sw.Write($"!!!!!!!!! You might meet a HEISENBUG\n!!!!!!!!! Please DO submit the following log to developers:\n{downloadLog}");
                return EFailReason.LockingFailed;
            }
            return EFailReason.Unknown;
        }

        // 解析自己 (注意: 如果要解析多个物品，请使用 ItemList.ParseAll(), 此方法主要用于测试)
        public async Task Parse() {
            ParseState = EParseState.Handling;
            var response = await API.SwDownloader.Request([ItemId]);
            if (response == null || response?.Length == 0) {
                LogManager.Log.Error("Failed to parse item {ItemId}", ItemId);
                return;
            }
            ParseWith(response![0]);
        }

        // 检查信息是否完备
        public bool IsCompleted() {
            return !string.IsNullOrEmpty(ItemId) &&
                !string.IsNullOrEmpty(ItemTitle) &&
                ItemSize != 0 && AppId != 0 &&
                !string.IsNullOrEmpty(AppName) &&
                !string.IsNullOrEmpty(UrlPreview);
        }
    }
}
