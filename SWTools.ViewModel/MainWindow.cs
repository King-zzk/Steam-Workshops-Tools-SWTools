using Semver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace SWTools.ViewModel {
    public partial class MainWindow : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // 总列表
        private Core.ItemList items = [];
        // 绑定
        public ObservableCollection<DisplayItem> DisplayItems { get; set; } = [];

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

        public MainWindow() {
            // 加载下载列表
            if (!File.Exists(Core.Constants.DownloadListFile)) {
                Core.LogManager.Log?.Warning("{Filename} not found, skipping loading",
                    Core.Constants.DownloadListFile);
                return;
            }
            items = Core.ItemList.Load(Core.Constants.DownloadListFile) ?? [];
            items.CheckDownloadedItems();
            UpdateDisplay();
            // 注册事件
            items.CollectionChanged += (s, e) => {
                UpdateDisplay();
            };
        }

        public void CleanUp() {
            if (!Directory.Exists(Core.Constants.CommonDir)) {
                Directory.CreateDirectory(Core.Constants.CommonDir);
            }
            items.Save(Core.Constants.DownloadListFile);
        }

        // 获取仓库最新信息 (返回公告)
        public async Task<string?> FetchRepo() {
            // 开始
            string statusText = "正在从仓库拉取最新信息...（您可以在此期间添加下载任务）";
            StatusText = statusText;
            IsBtnStartEnable = false;
            IsIndeterminate = true;

            if (!Directory.Exists(Core.Constants.CommonDir)) {
                Directory.CreateDirectory(Core.Constants.CommonDir);
            }

            // 拉取最新信息
            if (!await Core.API.LatestInfo.Fetch(Core.Constants.LatestInfoFile)) {
                StatusText = "拉取仓库最新信息失败，请检查网络连接（重启程序以重试）";
            }
            var info = Core.Helper.Main.ReadLatestInfo();
            if (info?.Release != null &&
                SemVersion.Parse(info.Release).CompareSortOrderTo(Core.Constants.Version) > 0) {
                StatusText = "检测到新的发行版。在 “更多” 查看详情";
            } else if (info?.PreRelease != null &&
                SemVersion.Parse(info.PreRelease).CompareSortOrderTo(Core.Constants.Version) > 0) {
                StatusText = "检测到新的预发行版。在 “更多” 查看详情";
            }

            // 拉取公有账户池
            if (await Core.API.PubAccounts.Fetch(Core.Constants.PubAccountsFile)) {
                Core.AccountManager.LoadPub();
            } else {
                StatusText = "拉取公有账户池失败，请检查网络连接（重启程序以重试）";
            }

            // 拉取公告
            string? notice;
            notice = await Core.API.Notice.Request();
            if (string.IsNullOrEmpty(notice)) {
                StatusText = "拉取公告失败，请检查网络连接（重启程序以重试）";
            } else if (File.Exists(Core.Constants.NoticeFile)) {
                string lastNotice;
                using StreamReader sr = new(Core.Constants.NoticeFile);
                lastNotice = sr.ReadToEnd();
                // 公告内容一样就不显示了
                if (notice == lastNotice) notice = null;
            }
            if (!string.IsNullOrEmpty(notice)) {
                if (!Directory.Exists(Core.Constants.CacheDir)) {
                    Directory.CreateDirectory(Core.Constants.CacheDir);
                }
                using StreamWriter sw = new(Core.Constants.NoticeFile, false);
                sw.Write(notice);
            }

            if (StatusText == statusText) { // 正常结束
                StatusText = StatusTextDefault;
            }
            // 结束
            IsBtnStartEnable = true;
            IsIndeterminate = false;

            return notice;
        }

        // 更新绑定 (DisplayItems)
        public void UpdateDisplay() {
            DisplayItems.Clear();
            foreach (var item in items) {
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
