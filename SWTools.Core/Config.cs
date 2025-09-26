using PropertyChanged;
using Serilog;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.Json;

namespace SWTools.Core {
    /// <summary>
    /// 可自定义的配置
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class Config : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // 空配置
        public static readonly Config Empty = new();

        // [常规]
        public bool IgnoreMissingFiles { get; set; } = false;   // 忽略丢失的文件
        public bool NoAutoFetch { get; set; } = false;          // 禁用自动更新

        // [调试选项]
#if DEBUG
        public bool LogDebug { get; set; } = true;      // 输出调试日志
#else
        public bool LogDebug { get; set; } = false;     // 输出调试日志
#endif
        public bool NoCache { get; set; } = false;      // 禁用缓存

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
