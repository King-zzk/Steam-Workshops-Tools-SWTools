using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWTools.ViewModel {
    public class AddTaskWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 用户输入的要下载的项
        private string _itemsToDownload = "";
        public string ItemsToDownload {
            get => _itemsToDownload;
            set {
                if (_itemsToDownload == value) return;
                _itemsToDownload = value;
                OnPropertyChanged(nameof(ItemsToDownload));
            }
        }
        // 解析按钮
        private bool _isBtnParseEnable = false;
        public bool IsBtnParseEnable {
            get => _isBtnParseEnable;
            set {
                if (_isBtnParseEnable == value) return;
                _isBtnParseEnable = value;
                OnPropertyChanged(nameof(IsBtnParseEnable));
            }
        }
        // 进度条
        private bool _isIndeterminate = false;
        public bool IsIndeterminate {
            get => _isIndeterminate;
            set {
                if (_isIndeterminate == value) return;
                _isIndeterminate = value;
                OnPropertyChanged(nameof(IsIndeterminate));
            }
        }
        // 解析列表
        private Core.ItemList _itemList = [];
        public Core.ItemList Items {
            get => _itemList;
            set {
                if (_itemList == value) return;
                _itemList = value;
                UpdateDisplay();
            }
        }
        // 修改按钮
        private bool _isBtnEditEnable = false;
        public bool IsBtnEditEnable {
            get => _isBtnEditEnable;
            set {
                if (_isBtnEditEnable == value) return;
                _isBtnEditEnable = value;
                OnPropertyChanged(nameof(IsBtnEditEnable));
            }
        }
        // 绑定
        public ObservableCollection<DisplayItem> DisplayItems { get; set; } = [];

        public AddTaskWindow() {
            // 注册事件
            _itemList.CollectionChanged += (s, e) => {
                UpdateDisplay();
            };
        }

        // 检查字符串是否只含合法字符
        public bool CheckString(in string str) {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, "^[0-9\r\n]*$");
        }

        // 添加到解析列表
        public bool AddToQueue() {
            var items = ItemsToDownload.Split('\n');
            foreach (var item in items) {
                var itemTrimed = item.Trim();
                var newItem = new Core.Item(itemTrimed);
                newItem.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                Items.Add(newItem);
            }
            ItemsToDownload = string.Empty;
            // 检查一下是不是真的添加了
            bool hasNew = false;
            foreach (var item in Items) {
                if (item.ParseState == Core.Item.EParseState.InQueue) {
                    hasNew = true;
                    break;
                }
            }
            return hasNew;
        }

        // 启动解析
        public async void StartParse() {
            await Items.ParseAll();
        }

        // 是否有正在解析的
        public bool HasParsing() {
            bool hasHandling = false;
            foreach (var item in Items) {
                if (item.ParseState == Core.Item.EParseState.Handling) {
                    hasHandling = true;
                    break;
                }
            }
            return hasHandling;
        }

        // 是否有解析失败的
        public bool HasFailed() {
            bool hasFailed = false;
            foreach (var item in Items) {
                if (item.ParseState == Core.Item.EParseState.Failed) {
                    hasFailed = true;
                    break;
                }
            }
            return hasFailed;
        }

        // 更新绑定
        private void UpdateDisplay() {
            DisplayItems.Clear();
            foreach (var item in _itemList) {
                DisplayItem displayItem = new(item, true);
                DisplayItems.Add(displayItem);
            }
            IsIndeterminate = HasParsing();
        }

        public async void RetryParse() {
            for (var i = 0; i < Items.Count; i++) {
                if (Items[i].ParseState != Core.Item.EParseState.Done) {
                    Items[i].ParseState = Core.Item.EParseState.InQueue;
                }
            }
            await Items.ParseAll();
        }
    }
}
