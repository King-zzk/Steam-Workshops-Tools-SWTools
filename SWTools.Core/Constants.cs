using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace SWTools.Core {
    /// <summary>
    ///  包含了目录、URL、Helper 里面的常量
    /// </summary>
    public record Constants {
        /* 目录 */
        public const string ConfigDirName = "data/";
        public const string ConfigFileName = "data/Config.json";
        public const string LogDirName = "logs/";
        public const string LogFileName = "logs/latest.log";
        public const string PreviewDirName = "cache/previews/";

        /* URL */
        public const string UrlVersion = "https://raw.githubusercontent.com/King-zzk/king-zzk.github.io/refs/heads/main/version.txt";
        public const string UrlSteamcmd = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
        public static readonly string[] UrlGithubProxy = [
            "", // 这个空的不能删
            "https://gh.llkk.cc/",
            "https://gitproxy.click/",
            "https://ghproxy.net/"
        ];

        /* 日志 */
        public const string LogTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
        public const string LogTemplateDebug = "[{Timestamp:HH:mm:ss} {Level:u3}] {Namespace}.{Method}: {Message:lj}{NewLine}{Exception}";

        // Json 选项
        public static readonly JsonSerializerOptions JsonOptions = new() {
            // 序列化选项
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
            // 反序列化选项
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        };

    }
}
