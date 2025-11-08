using System.ComponentModel;

namespace SWTools.ViewModel {
    public class MoreWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 配置
        public Core.Config Config {
            get { return Core.ConfigManager.Config; }
            set {
                Core.ConfigManager.Config = value;
                OnPropertyChanged(nameof(Config));
            }
        }

        // 版本
        public string Version { get; set; } = Core.Constants.Version.ToString();
        public string PubVersion { get; set; } = Core.AccountManager.PubVersion;

        // 是否提醒过用户新版本
        public static bool HasHintedLatestVersion { get; set; } = false;
    }
}
