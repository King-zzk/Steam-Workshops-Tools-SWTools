using System;

namespace SWTools.Core {
    /// <summary>
    /// (用于下载的) Steam 账户
    /// </summary>
    public record Account {
        public string Name { get; set; } = "";      // 账户名
        public string Password { get; set; } = "";  // 密码
        public long[] AppIds { get; set; } = [];    // 支持的 App
    }
}
