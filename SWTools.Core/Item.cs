using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品
    /// </summary>
    public partial class Item {
        public string ItemId { get; set; } = "";      // 物品 Id
        public string ItemTitle { get; set; } = "";   // 物品标题
        public long ItemSize { get; set; }            // 该物品的文件大小
        public long AppId { get; set; }               // 物品属于的 App 的 Id
        public string AppName { get; set; } = "";     // 物品属于的 App 的名字

        // 解析状态
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EParseState ParseState { get; set; } = EParseState.InQueue;

        // 下载状态
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EDownloadState DownloadState { get; set; } = EDownloadState.InQueue;

        // 下载失败原因
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EFailReason FailReason { get; set; } = EFailReason.Null;
        private string _exceptionMsg = "";

        // 解析之后的操作
        [JsonIgnore]
        public EAfterParse AfterParse { get; set; } = EAfterParse.Nothing; 

        public enum EParseState {
            InQueue, Handling, Done, Failed,
            Manual // 指示物品信息是用户自己指定的
        }
        public enum EDownloadState {
            InQueue, Handling, Done, Failed, Missing
        }
        public enum EFailReason {
            Null, Unknown, Exception,
            FileNotFound, Timeout, NoConnection,
            AccountDisabled, InvalidPassword, NoMatch
        }
        public enum EAfterParse {
            Nothing, Download
        }

        // 空物品
        public static readonly Item Empty = new();

        public Item() { }
        public Item(string itemId) {
            ItemId = itemId;
        }

        // 序列化到 Json
        public override string ToString() {
            try {
                return JsonSerializer.Serialize(this, Helper._jsonOptions);
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when serializing Json:\n{Exception}", ex);
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
                    return "未知错误";
                case EFailReason.FileNotFound:
                    return "物品未找到，请检查物品 ID 是否正确";
                case EFailReason.Timeout:
                    return "下载超时，请重试";
                case EFailReason.NoConnection:
                    return "无网络连接";
                case EFailReason.AccountDisabled:
                    return "账号不可用，请尝试向开发者反映此问题";
                case EFailReason.InvalidPassword:
                    return "密码错误，请尝试向开发者反映此问题";
                case EFailReason.NoMatch:
                    return "Steam App 与物品 ID 不匹配";
            }
            Log.Logger.Error("Received unknown enum value: {FailReason}", FailReason);
            return string.Empty;
        }

        // 获取下载文件目录
        public string GetDownloadPath() {
            StringBuilder sb = new();
            sb.Append("steamcmd\\steamapps\\workshop\\content\\");
            sb.Append(AppId).Append('\\');
            sb.Append(ItemId).Append('\\');
            return sb.ToString();
        }

        // 把状态二值化到 InQueue / Done
        public void FixState() {
            if (ParseState != EParseState.Done) {
                ParseState = EParseState.InQueue;
            }
            if (DownloadState != EDownloadState.Done) {
                DownloadState = EDownloadState.InQueue;
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
            }
            return EFailReason.Unknown;
        }
    }
}
