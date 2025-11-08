namespace SWTools.Core.API {
    /// <summary>
    /// 公告 api/notice
    /// </summary>
    public class Notice {
        // API 地址
        private const string _apiUrl = Constants.UrlRepoApi + "notice";

        // 请求 API (无异常)
        public static async Task<string?> Request() {
            string? response = await Helper.Http.MakeGithubGet(_apiUrl);
            return response;
        }
    }
}
