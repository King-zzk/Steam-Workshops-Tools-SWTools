﻿using System;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 辅助方法 (Http)
    /// </summary>
    public static partial class Helper {
        // 发送 Http Get 请求
        public static async Task<string> MakeHttpGet(string url) {
            using HttpClient client = new();
            try {
                // 发送请求
                var response = await client.GetAsync(url);
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
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                LogManager.Log.Error("Exception occured when requesting \"{Url}\" (GET):\n{Exception}",
                    url, ex);
                return string.Empty;
            }
        }

        // 发送 Http Post 请求
        public static async Task<string> MakeHttpPost(string url, string content) {
            using HttpClient client = new();
            try {
                // 发送请求
                using StringContent strContent = new(content);
                var response = await client.PostAsync(url, strContent);
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
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                LogManager.Log.Error("Exception occured when requesting \"{Url}\" (POST):\n{Exception}",
                    url, ex);
                LogManager.Log.Error("The POST content was: \"{Content}\"",
                    content);
                return string.Empty;
            }
        }

        // 下载文件到指定目录
        public static async Task<bool> DownloadFile(string url, string filePath) {
            using HttpClient client = new();
            try {
                // 发送请求
                HttpResponseMessage response = await client.GetAsync(url);
                // 接收
                response.EnsureSuccessStatusCode();
                Stream contentStream = await response.Content.ReadAsStreamAsync();
                // 写入
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                await contentStream.CopyToAsync(fileStream);
                LogManager.Log.Information("Downloaded \"{Url}\" to \"{FilePath}\"",
                    url, filePath);
                return true;
            } catch (Exception ex) {
                LogManager.Log.Error("Exception occured when downloading \"{Url}\":\n{Exception}",
                    url, ex);
                return false;
            }
        }

        // 下载图片 (自动判断后缀名, 返回文件路径)
        public static async Task<string?> DownloadImage(string url, string filePath) {
            using HttpClient client = new();
            try {
                // 发送请求
                //var task = client.GetAsync(url); // NOTE: 这里用 await 的话，ViewModel 调用 .Wait() 会卡住
                //task.Wait();
                HttpResponseMessage response = await client.GetAsync(url);
                // 接收
                response.EnsureSuccessStatusCode();
                Stream contentStream = await response.Content.ReadAsStreamAsync();
                // 判断后缀名
                var contentType = response.Content.Headers.ContentType;
                if (contentType == null) {
                    throw new Exception("response.Content.Headers.ContentType is null");
                }
                if (contentType.MediaType == null) {
                    throw new Exception("response.Content.Headers.ContentType.MediaType is null");
                }
                if (!contentType.MediaType.StartsWith("image/")) {
                    throw new Exception("Content is not image");
                }
                filePath += '.' + contentType.MediaType.Split('/')[1];
                // 写入
                using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                await contentStream.CopyToAsync(fileStream);
                LogManager.Log.Information("Downloaded \"{Url}\" to \"{FilePath}\"",
                    url, filePath);
                return filePath;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when downloading \"{Url}\":\n{Exception}",
                    url, ex);
                return null;
            }
        }
    }
}
