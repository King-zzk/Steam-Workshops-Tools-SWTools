using System.Text;
using System.Text.Json;

namespace SWTools.Core.API {
    /// <summary>
    /// 获取物品信息
    /// GetPublishedFileDetails
    /// https://partner.steamgames.com/doc/webapi/isteamremotestorage#GetPublishedFileDetails
    /// </summary>
    public static class GetPublishedFileDetails {
        // 请求 API
        public static async Task<Response?> Request(List<string> itemIds) {
            if (itemIds.Count == 0) return null;
            // 构造请求
            var formData = new List<KeyValuePair<string, string>> {
                new("itemcount", itemIds.Count.ToString()),
            };
            for (var i = 0; i < itemIds.Count; i++) {
                formData.Add(new KeyValuePair<string, string>($"publishedfileids[{i}]",
                    itemIds[i]));
            }
            HttpContent content = new FormUrlEncodedContent(formData);
            // 发送请求
            LogManager.Log.Information("Requesting GetPublishedFileDetails");
            string? response = await Helper.Http.MakeHttpPost(_apiUrl, content);
            if (response == null) return null;
            LogManager.Log.Information("Requested GetPublishedFileDetails successfully");
            // 处理回复
            try {
                Root? root = JsonSerializer.Deserialize<Root>(response, Constants.JsonOptions);
                if (root == null) return null;
                return root.response;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when deserializing Json:\n{Exception}", ex);
            }
            return null;
        }

        // API 地址
        private const string _apiUrl = "https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/";

        // API 响应包
        public record Root { public Response? response { get; set; } }
        public record Response {
            public long result { get; set; }
            public long resultcount { get; set; }
            public Publishedfiledetails[]? publishedfiledetails { get; set; }

            public record Publishedfiledetails {
                public string? publishedfileid { get; set; }
                public long result { get; set; }
                public string? creator { get; set; }
                public long creator_app_id { get; set; }
                public long consumer_app_id { get; set; }
                public string? filename { get; set; }
                public string? file_size { get; set; }
                public string? file_url { get; set; }
                public string? hcontent_file { get; set; }
                public string? preview_url { get; set; }
                public string? hcontent_preview { get; set; }
                public string? title { get; set; }
                public string? description { get; set; }
                public long time_created { get; set; }
                public long time_updated { get; set; }
                public long visibility { get; set; }
                public long banned { get; set; }
                public string? ban_reason { get; set; }
                public long subscriptions { get; set; }
                public long favorited { get; set; }
                public long lifetime_subscriptions { get; set; }
                public long lifetime_favorited { get; set; }
                public long views { get; set; }
                public Tag[]? tags { get; set; }

                public record Tag { public string? tag; }
            }
        }
    }
}
