using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
