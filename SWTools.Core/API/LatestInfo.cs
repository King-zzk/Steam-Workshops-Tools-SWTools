using System.Text.Json;

namespace SWTools.Core.API {
    /// <summary>
    /// 仓库最新信息 api/latest_info
    /// </summary>
    public static class LatestInfo {
        // 请求 API
        public static async Task<Response?> Request() {
            string? response = await Helper.Http.MakeGithubGet(_apiUrl);
            if (response == null) return null;
            // 处理回复
            try {
                return JsonSerializer.Deserialize<Response>(response, Constants.JsonOptions);
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when deserializing Json:\n{Exception}", ex);
            }
            return null;
        }

        // 请求并保存到文件
        public static async Task<bool> Fetch(string filename) {
            LogManager.Log.Information("Fetching latest_info");
            var response = await Request();
            if (response == null) return false;
            try {
                // 写入文件
                using (StreamWriter sw = new(filename)) {
                    sw.Write(response.ToString());
                }
                return true;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when saving {FileName}:\n{Exception}",
                    filename, ex);
                return false;
            }
        }

        // API 地址
        private const string _apiUrl = Constants.UrlRepoApi + "latest_info";

        // API 响应包
        public record Response {
            public string? Release { get; set; }       // 最新的正式版
            public string? PreRelease { get; set; }    // 最新的发行版

            public string? PubAccountsVersion { get; set; }  // 最新的公有账户版本
            public long[]? PriAccountsAppIds { get; set; }   // 最新的私有账户支持的 AppID

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
