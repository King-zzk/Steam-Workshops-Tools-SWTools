﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Semver;

namespace SWTools.ViewModel {
    public class MainWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 下载列表
        private Core.ItemList _itemList = [];
        public Core.ItemList Items {
            get => _itemList;
            set {
                if (_itemList == value) return;
                _itemList = value;
                UpdateDisplay();
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
        // 状态栏
        public const string StatusTextDefault = "就绪";
        private string _statusText = StatusTextDefault;
        public string StatusText {
            get => _statusText;
            set {
                if (_statusText == value) return;
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
        // 添加任务按钮
        private bool _isBtnAddTaskEnable = true;
        public bool IsBtnAddTaskEnable {
            get => _isBtnAddTaskEnable;
            set {
                if (_isBtnAddTaskEnable == value) return;
                _isBtnAddTaskEnable = value;
                OnPropertyChanged(nameof(IsBtnAddTaskEnable));
            }
        }
        // 启动按钮
        private bool _isBtnStartEnable = true;
        public bool IsBtnStartEnable {
            get => _isBtnStartEnable;
            set {
                if (_isBtnStartEnable == value) return;
                _isBtnStartEnable = value;
                OnPropertyChanged(nameof(IsBtnStartEnable));
            }
        }
        // 暂停按钮
        private bool _isBtnStopEnable = false;
        public bool IsBtnStopEnable {
            get => _isBtnStopEnable;
            set {
                if (_isBtnStopEnable == value) return;
                _isBtnStopEnable = value;
                OnPropertyChanged(nameof(IsBtnStopEnable));
            }
        }
        // 清空按钮
        private bool _isBtnRemoveAllEnable = true;
        public bool IsBtnRemoveAllEnable {
            get => _isBtnRemoveAllEnable;
            set {
                if (_isBtnRemoveAllEnable == value) return;
                _isBtnRemoveAllEnable = value;
                OnPropertyChanged(nameof(IsBtnRemoveAllEnable));
            }
        }
        // 绑定
        public ObservableCollection<DisplayItem> DisplayItems { get; set; } = [];

        // 是否要执行下载
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

        // 下载单个物品
        public async Task DownloadOne(string itemId) {
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
                if (DisplayItems[i].Item.DownloadState != Core.Item.EDownloadState.InQueue ||
                    DisplayItems[i].Item.ItemId != itemId) {
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

        // 获取仓库最新信息
        public async void FetchRepo() {
            // 开始
            StatusText = "正在从仓库拉取最新信息...（您可以在此期间添加下载任务）";
            IsBtnStartEnable = false;
            IsIndeterminate = true;

            if (!Directory.Exists(Core.Constants.CommonDir)) {
                Directory.CreateDirectory(Core.Constants.CommonDir);
            }
            await Core.API.LatestInfo.Fetch(Core.Constants.LatestInfoFile);
            var info = Core.Helper.Main.ReadLatestInfo();
            if (await Core.API.PubAccounts.Fetch(Core.Constants.PubAccountsFile)) {
                Core.AccountManager.LoadPub();
            }

            // 结束
            if (info?.Release != null &&
                SemVersion.Parse(info.Release).CompareSortOrderTo(Core.Constants.Version) > 0) {
                StatusText = $"检测到新的发行版。在 “更多” 查看详情";
            } else if (info?.PreRelease != null &&
                SemVersion.Parse(info.PreRelease).CompareSortOrderTo(Core.Constants.Version) > 0) {
                StatusText = $"检测到新的预发行版。在 “更多” 查看详情";
            } else {
                StatusText = StatusTextDefault;
            }
            IsBtnStartEnable = true;
            IsIndeterminate = false;
        }

        // 更新绑定
        public void UpdateDisplay() {
            DisplayItems.Clear();
            foreach (var item in _itemList) {
                DisplayItem displayItem = new(item, false);
                displayItem.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                if (string.IsNullOrEmpty(displayItem.PreviewImage) ||
                    displayItem.PreviewImage == DisplayItem.PreviewImageDefault) {
                    _ = displayItem.DownloadPreviewImage();
                }
                DisplayItems.Add(displayItem);
            }
        }
    }
}
