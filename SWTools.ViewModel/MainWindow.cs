using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWTools.ViewModel {
    public class MainWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 下载列表
        private Core.ItemList _itemList = [];
        public Core.ItemList Items {
            get { return _itemList; }
            set {
                if (_itemList == value) return;
                _itemList = value;
                UpdateDisplay();
            }
        }
        // 绑定
        public ObservableCollection<DisplayItem> DisplayItems { get; set; } = [];

        public MainWindow() {
            // 注册事件
            _itemList.CollectionChanged += (s, e) => {
                UpdateDisplay();
            };
        }

        // 添加测试数据
        public async Task AddDebugData() {
            Core.Item item = new("3543159422");
            await item.Parse();
            Items.Add(item);
        }

        // 更新绑定
        private void UpdateDisplay() {
            DisplayItems.Clear();
            foreach (var item in _itemList) {
                DisplayItem displayItem = new(item, false);
                displayItem.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                if (string.IsNullOrEmpty(displayItem.PreviewImage)) {
                    _ = displayItem.DownloadPreviewImage();
                }
                DisplayItems.Add(displayItem);
            }
        }
    }
}
