using System.Text;
using System.Text.Json;

namespace SWTools.Core.API {
    /// <summary>
    /// steamworkshopdownloader.io/api
    /// </summary>
    public static class SwDownloader {
        // 请求 API
        public static async Task<Response[]?> Request(List<string> itemIds) {
            if (itemIds.Count == 0) return null;
            // 构造请求
            StringBuilder sb = new();
            sb.Append('[');
            foreach (var item in itemIds) {
                if (sb.Length > 1)
                    sb.Append(',');
                sb.Append(item);
            }
            sb.Append(']');
            // 发送请求
            LogManager.Log.Information("Requesting steamworkshopdownloader.io/api");
            string? response = await Helper.Http.MakeHttpPost(_apiUrl, sb.ToString());
            if (response == null) return null;
            LogManager.Log.Information("Requested steamworkshopdownloader.io/api successfully");
            // 处理回复
            try {
                return JsonSerializer.Deserialize<Response[]>(response, Constants.JsonOptions);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when deserializing Json:\n{Exception}", ex);
            }
            return null;
        }

        // API 地址
        private const string _apiUrl = "https://steamworkshopdownloader.io/api/details/file";

        // API 响应包
        public record Response {
            public long result { get; set; }
            public string? publishedfileid { get; set; }
            public string? creator { get; set; }
            public long creator_appid { get; set; }
            public long consumer_appid { get; set; }
            public long consumer_shortcutid { get; set; }
            public string? filename { get; set; }
            public string? file_size { get; set; }
            public string? preview_file_size { get; set; }
            public string? file_url { get; set; }
            public string? preview_url { get; set; }
            public string? url { get; set; }
            public string? hcontent_file { get; set; }
            public string? hcontent_preview { get; set; }
            public string? title { get; set; }
            public string? title_disk_safe { get; set; }
            public string? file_description { get; set; }
            public long time_created { get; set; }
            public long time_updated { get; set; }
            public long visibility { get; set; }
            public long flags { get; set; }
            public bool workshop_file { get; set; }
            public bool workshop_accepted { get; set; }
            public bool show_subscribe_all { get; set; }
            public long num_comments_developer { get; set; }
            public long num_comments_public { get; set; }
            public bool banned { get; set; }
            public string? ban_reason { get; set; }
            public string? banner { get; set; }
            public bool can_be_deleted { get; set; }
            public bool incompatible { get; set; }
            public string? app_name { get; set; }
            public long file_type { get; set; }
            public bool can_subscribe { get; set; }
            public long subscriptions { get; set; }
            public long favorited { get; set; }
            public long followers { get; set; }
            public long lifetime_subscriptions { get; set; }
            public long lifetime_favorited { get; set; }
            public long lifetime_followers { get; set; }
            public string? lifetime_playtime { get; set; }
            public string? lifetime_playtime_sessions { get; set; }
            public long views { get; set; }
            public bool spoiler_tag { get; set; }
            public long num_children { get; set; }
            public object? children { get; set; }
            public long num_reports { get; set; }
            public object? previews { get; set; }
            public Tag[]? tags { get; set; }
            public VoteData? vote_data { get; set; }
            public long language { get; set; }

            public record Tag {
                public string? tag { get; set; }
                public bool adminonly { get; set; }
            }

            public record VoteData {
                public long result { get; set; }
                public long votes_up { get; set; }
                public long votes_down { get; set; }
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
        }
    }
}
