using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Semver;

namespace SWTools.Core {
    /// <summary>
    ///  包含了目录、URL、Helper 里面的常量
    /// </summary>
    public static class Constants {
        /* 版本 */
        public static readonly SemVersion Version = SemVersion.Parse("2.0.0-beta.1", SemVersionStyles.Strict);

        /* 目录 */
        // 公共数据
        public const string CommonDir = "data/";
        public const string ConfigFile = CommonDir + "config.json"; // 配置
        public const string DownloadListFile = CommonDir + "download.json"; // 下载列表
        // 日志
        public const string LogDir = "logs/";
        public const string LogFile = LogDir + "latest.log";
        // 缓存
        public const string CacheDir = "cache/";
        public const string PreviewDir = CacheDir + "previews/";
        public const string CacheParseFile = CacheDir + "parse.json";
        // Steamcmd
        public const string SteamcmdDir = "steamcmd/";
        public const string SteamcmdFile = SteamcmdDir + "steamcmd.exe";

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
