using System;
using System.IO;
using Serilog;
using static SWTools.Core.SwdApi;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (Http)
    /// </summary>
    public static partial class Helper {
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

        // 下载文件到指定目录
        public static bool DownloadFile(in string url, in string filePath) {
            using HttpClient client = new();
            try {
                // 发送请求
                HttpResponseMessage response;
                using (var task = client.GetAsync(url)) {
                    task.Wait();
                    response = task.Result;
                    response.EnsureSuccessStatusCode();
                }
                // 接收
                Stream contentStream;
                using (var task = response.Content.ReadAsStreamAsync()) {
                    task.Wait();
                    contentStream = task.Result;
                }
                // 写入
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                using (var task = contentStream.CopyToAsync(fileStream)) {
                    task.Wait();
                }
                Log.Logger.Information("Downloaded \"{Url}\" to \"{FilePath}\"",
                    url, filePath);
                return true;
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when downloading \"{Url}\":\n{Exception}",
                    url, ex);
                return false;
            }
        }
    }
}
