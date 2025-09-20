using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        // 进度条
        private bool _isIndeterminate = false;
        public bool IsIndeterminate {
            get { return _isIndeterminate; }
            set {
                if (_isIndeterminate == value) return;
                _isIndeterminate = value;
                OnPropertyChanged(nameof(IsIndeterminate));
            }
        }
        // 状态栏
        private string _statusText = "提示：单击 “添加下载任务” 添加要下载的物品";
        public string StatusText {
            get { return _statusText; }
            set {
                if (_statusText == value) return;
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
        // 添加任务按钮
        private bool _isBtnAddTaskEnable = true;
        public bool IsBtnAddTaskEnable {
            get { return _isBtnAddTaskEnable; }
            set {
                if (_isBtnAddTaskEnable == value) return;
                _isBtnAddTaskEnable = value;
                OnPropertyChanged(nameof(IsBtnAddTaskEnable));
            }
        }
        // 启动按钮
        private bool _isBtnStartEnable = true;
        public bool IsBtnStartEnable {
            get { return _isBtnStartEnable; }
            set {
                if (_isBtnStartEnable == value) return;
                _isBtnStartEnable = value;
                OnPropertyChanged(nameof(IsBtnStartEnable));
            }
        }
        // 暂停按钮
        private bool _isBtnStopEnable = false;
        public bool IsBtnStopEnable {
            get { return _isBtnStopEnable; }
            set {
                if (_isBtnStopEnable == value) return;
                _isBtnStopEnable = value;
                OnPropertyChanged(nameof(IsBtnStopEnable));
            }
        }
        // 清空按钮
        private bool _isBtnRemoveAllEnable = true;
        public bool IsBtnRemoveAllEnable {
            get { return _isBtnRemoveAllEnable; }
            set {
                if (_isBtnRemoveAllEnable == value) return;
                _isBtnRemoveAllEnable = value;
                OnPropertyChanged(nameof(IsBtnRemoveAllEnable));
            }
        }
        // 绑定
        public ObservableCollection<DisplayItem> DisplayItems { get; set; } = [];

        // 要下载吗
        public bool IsDownloading = false;

        public MainWindow() {
            // 加载下载列表
            _itemList = Core.ItemList.Load(Core.Constants.DownloadListFile) ?? [];
            _itemList.CheckDownloadedItems();
            UpdateDisplay();
            // 注册事件
            _itemList.CollectionChanged += (s, e) => {
                UpdateDisplay();
            };
        }

        public void CleanUp() {
            if (!Directory.Exists(Core.Constants.CommonDir)) {
                Directory.CreateDirectory(Core.Constants.CommonDir);
            }
            _itemList.Save(Core.Constants.DownloadListFile);
        }

        // 启动下载
        public async Task StartDownload() {
            // 开始
            IsDownloading = true;

            IsBtnAddTaskEnable = false;
            IsBtnStartEnable = false;
            IsBtnRemoveAllEnable = false;

            IsBtnStopEnable = true;
            IsIndeterminate = true;

            // 准备 Steamcmd
            StatusText = "正在准备 Steamcmd，请耐心等待（在日志中查看详细信息）";
            await Core.Helper.Steamcmd.Setup();
            // 开始下载
            for (var i = 0; i < DisplayItems.Count && IsDownloading; i++) {
                if (DisplayItems[i].Item.DownloadState != Core.Item.EDownloadState.InQueue) {
                    continue;
                }
                StatusText = $"正在下载 {DisplayItems[i].ItemName}";
                DisplayItems[i].Item.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                await DisplayItems[i].Item.Download();
            }

            // 结束
            StatusText = "已完成";
            IsDownloading = false;

            IsBtnAddTaskEnable = true;
            IsBtnStartEnable = true;
            IsBtnRemoveAllEnable = true;

            IsBtnStopEnable = false;
            IsIndeterminate = false;
        }

        // 更新绑定
        private void UpdateDisplay() {
            DisplayItems.Clear();
            bool hasInqueue = false; // 是否有等待下载的物品
            foreach (var item in _itemList) {
                if (item.DownloadState == Core.Item.EDownloadState.InQueue) {
                    hasInqueue = true;
                }
                DisplayItem displayItem = new(item, false);
                displayItem.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                if (string.IsNullOrEmpty(displayItem.PreviewImage)) {
                    _ = displayItem.DownloadPreviewImage();
                }
                DisplayItems.Add(displayItem);
            }
            // 状态栏
            if (hasInqueue && !IsDownloading) {
                StatusText = "提示：单击 “下载全部” 开始下载物品";
            }
        }
    }
}
