using Serilog.Context;
using SWTools.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SWTools.ViewModel {
    public class LogWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _logContent = string.Empty;
        private string _lastLogContent;
        private readonly DispatcherTimer _timer;

        public string LogContent {
            get => _logContent;
            set {
                _logContent = value;
                OnPropertyChanged(nameof(LogContent));
            }
        }

        public LogWindow() {
            // 初始化日志内容
            LogContent = LogManager.LogWriter.ToString();
            _lastLogContent = LogContent;

            // 设置定时器检查日志变化
            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _timer.Tick += CheckLogChanges;
            _timer.Start();
        }

        private void CheckLogChanges(object? sender, EventArgs e) {
            var currentContent = LogManager.LogWriter.ToString();
            if (currentContent != _lastLogContent) {
                LogContent = currentContent;
                _lastLogContent = currentContent;
            }
        }

        // 清理资源
        public void Cleanup() {
            _timer.Stop();
            _timer.Tick -= CheckLogChanges;
        }
    }
}
