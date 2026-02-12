using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace SWTools.ViewModel {
    /// <summary>
    /// 用于显示的创意工坊物品
    /// </summary>
    public class DisplayItem : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /* 用于列表显示 */

        public Core.Item Item { get; private set; }
        public string ItemName { get; set; } = "";
        public string ItemSize { get; set; } = "";
        public string AppName { get; set; } = "";
        public string Creator { get; set; } = "";
        public string State { get; set; } = "";

        public Brush Brush { get; set; }

        private string _previewImage = "";
        public string PreviewImage {
            get { return _previewImage; }
            set {
                if (_previewImage == value) return;
                _previewImage = value;
                OnPropertyChanged(nameof(PreviewImage));
            }
        }
        public const string PreviewImageDefault = "Resources/default-preview.png";

        /* 用于右键菜单 */
        public bool IsHandling { get { return Item.DownloadState == Core.Item.EDownloadState.Handling; } }
        public bool IsDownloaded { get { return Item.DownloadState == Core.Item.EDownloadState.Done; } }

        public DisplayItem(Core.Item item) {
            Item = item;
        }
        // 如果测重显示解析状态，则令 displayParseState = true;
        public DisplayItem(Core.Item item, bool displayParseState) {
            Item = item;
            // 设置信息
            if (string.IsNullOrEmpty(item.ItemTitle)) {
                ItemName = item.ItemId;
            } else {
                ItemName = item.ItemTitle;
            }
            if (string.IsNullOrEmpty(item.AppName)) {
                if (item.AppId == 0) AppName = "未知";
                else AppName = item.AppId.ToString();
            } else {
                AppName = item.AppName;
            }
            ItemSize = Helper.ByteToString(item.ItemSize);
            if (displayParseState) {
                if (item.ParseState == Core.Item.EParseState.Failed) {
                    State = "解析失败";
                    Brush = Brushes.Firebrick;
                } else if (item.ParseState == Core.Item.EParseState.Manual) {
                    State = "手动指定";
                    Brush = Brushes.DarkOliveGreen;
                } else if (item.ParseState == Core.Item.EParseState.Pending) {
                    State = "等待解析...";
                    Brush = Brushes.Black;
                } else if (item.ParseState == Core.Item.EParseState.Handling) {
                    State = "解析中...";
                    Brush = Brushes.DarkCyan;
                } else if (item.ParseState == Core.Item.EParseState.Done) {
                    State = "完成";
                    Brush = Brushes.DarkGreen;
                }
            } else {
                if (item.ParseState == Core.Item.EParseState.Failed) {
                    State = "信息解析失败";
                } else {
                    if (item.DownloadState == Core.Item.EDownloadState.Pending) {
                        State = "等待下载...";
                        Brush = Brushes.Black;
                    } else if (item.DownloadState == Core.Item.EDownloadState.Handling) {
                        State = "下载中...";
                        Brush = Brushes.DarkCyan;
                    } else if (item.DownloadState == Core.Item.EDownloadState.Missing) {
                        State = "文件丢失";
                        Brush = Brushes.Firebrick;
                    } else if (item.DownloadState == Core.Item.EDownloadState.Done) {
                        State = "✔ 完成";
                        Brush = Brushes.DarkGreen;
                    } else if (item.DownloadState == Core.Item.EDownloadState.Failed) {
                        State = "下载失败：" + item.GetFailMessage();
                        Brush = Brushes.Firebrick;
                    }
                }
            }
            // 从缓存加载缩略图 (必须，不受 Config.UseCaeche 影响)
            if (string.IsNullOrEmpty(item.UrlPreview)) {
                PreviewImage = PreviewImageDefault;
            }
            string? fileName = Core.Helper.Main.FindFileIgnoreExt(Core.Constants.PreviewDir, item.ItemId);
            if (fileName != null) {
                PreviewImage = Path.GetFullPath(Core.Constants.PreviewDir + fileName);
            } else {
                PreviewImage = PreviewImageDefault;
            }
        }

        private static List<string> _downloadingPreviews = [];
        // 下载缩略图
        public async Task DownloadPreviewImage() {
            if (_downloadingPreviews.Contains(Item.ItemId)) return;
            lock (_downloadingPreviews) {
                _downloadingPreviews.Add(Item.ItemId);
            }
            if (!Directory.Exists(Core.Constants.PreviewDir)) {
                Directory.CreateDirectory(Core.Constants.PreviewDir);
            }
            var res = await Core.Helper.Http.DownloadImage(Item.UrlPreview, Core.Constants.PreviewDir + Item.ItemId);
            if (res != null) {
                PreviewImage = Path.GetFullPath(res);
            }
            lock (_downloadingPreviews) {
                _downloadingPreviews.Remove(Item.ItemId);
            }
        }
    }
}