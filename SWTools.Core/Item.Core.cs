using System;
using System.Text.Json;
using Serilog;

namespace SWTools.Core {
    /// <summary>
    /// 创意工坊物品 (核心逻辑)
    /// </summary>
    public partial class Item {

        // 解析自己
        public void Parse() {
            ParseState = EParseState.Handling;
            string str = Helper.MakeHttpPost(SwdApi.Url, $"[{ItemId}]");
            if (str == "") {
                Log.Logger.Error("Failed to parse item {ItemId}: empty response", ItemId);
            }
            // 处理回复
            try {
                var response = JsonSerializer.Deserialize<SwdApi.Response[]>(str, Helper._jsonOptions);
                if (response == null) throw new Exception("response is null");
                ParseWith(response[0]);
            }
            catch (Exception ex) {
                Log.Logger.Error("Exception occured when deserializing Json:\n{Exception}", ex);
                ParseState = EParseState.Failed;
            }
        }
        // 解析 steamworkshopdownloader.io/api 返回的数据
        public void ParseWith(in SwdApi.Response swdResponse) {
            ItemTitle = swdResponse.title;
            ItemSize = long.Parse(swdResponse.file_size);
            AppId = swdResponse.consumer_appid;
            AppName = swdResponse.app_name;
            // 设置状态
            ParseState = EParseState.Done;
        }
    }
}