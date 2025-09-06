using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (Http)
    /// </summary>
    public partial class Helper {
        public static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
            // 序列化选项
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
            // 反序列化选项
            PropertyNameCaseInsensitive = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        };

        public static readonly string[] _githubProxies = {
            "", // 这个空的不能删
            "https://gh.llkk.cc/",
            "https://gitproxy.click/",
            "https://ghproxy.net/"
            // 3 个够了
        };

        // 发送 Http Get 请求
        public static string MakeHttpGet(string url) {
            using HttpClient client = new();
            try {
                // 发送请求
                var task = client.GetAsync(url);
                task.Wait();
                var response = task.Result;

                // 检查回复
                response.EnsureSuccessStatusCode();
                var contentType = response.Content.Headers.ContentType;
                if (contentType == null) {
                    throw new Exception("response.Content.Headers.ContentType is null");
                }
                if (contentType.MediaType == null) {
                    throw new Exception("response.Content.Headers.ContentType.MediaType is null");
                }
                if (!contentType.MediaType.StartsWith("text/")) {
                    throw new Exception("Content is not text");
                }
                var taskRead = response.Content.ReadAsStringAsync();
                taskRead.Wait();
                return taskRead.Result;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when requesting \"{Url}\":\n{Exception}",
                    url, ex);
                return string.Empty;
            }
        }

        // 发送 Http Post 请求
        public static string MakeHttpPost(string url, string content) {
            using HttpClient client = new();
            try {
                // 发送请求
                using StringContent strContent = new(content);
                var task = client.PostAsync(url, strContent);
                task.Wait();
                var response = task.Result;

                // 检查回复
                response.EnsureSuccessStatusCode();
                var contentType = response.Content.Headers.ContentType;
                if (contentType == null) {
                    throw new Exception("response.Content.Headers.ContentType is null");
                }
                if (contentType.MediaType == null) {
                    throw new Exception("response.Content.Headers.ContentType.MediaType is null");
                }
                if (!contentType.MediaType.StartsWith("text/")) {
                    throw new Exception("Content is not text");
                }
                var taskRead = response.Content.ReadAsStringAsync();
                taskRead.Wait();
                return taskRead.Result;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when requesting \"{Url}\":\n{Exception}",
                    url, ex);
                return string.Empty;
            }
        }
    }
}
