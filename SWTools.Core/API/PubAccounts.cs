﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWTools.Core.API {
    /// <summary>
    /// 公有账户 api/pub_accounts
    /// </summary>
    public static class PubAccounts {
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
            LogManager.Log.Information("Fetching pub_accounts");
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
        private const string _apiUrl = Constants.UrlRepoApi + "pub_accounts";

        // API 响应包
        public record Response {
            public string? Version { get; set; }
            public Account[]? Accounts { get; set; }

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
