using System;
using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// (用于下载的) Steam 账户
    /// </summary>
    public record Account {
        public string Name { get; set; } = "";      // 账户名
        public string Password { get; set; } = "";  // 密码
        public long[] AppIds { get; set; } = [];    // 支持的 App
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
