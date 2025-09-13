using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.IO;

namespace SWTools.ViewModel {
    /// <summary>
    /// 用于显示的创意工坊物品
    /// </summary>
    public class DisplayItem : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Core.Item Item { get; private set; }
        public string ItemName { get; set; } = "";
        public string ItemSize { get; set; } = "";
        public string AppName { get; set; } = "";
        public string State { get; set; } = "";
        private string _previewImage = "";
        public string PreviewImage {
            get { return _previewImage; }
            set {
                if (_previewImage == value) return;
                _previewImage = value;
                OnPropertyChanged(nameof(PreviewImage));
            }
        }

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
                } else if (item.ParseState == Core.Item.EParseState.Manual) {
                    State = "手动指定";
                } else if (item.ParseState == Core.Item.EParseState.InQueue) {
                    State = "等待解析...";
                } else if (item.ParseState == Core.Item.EParseState.Handling) {
                    State = "解析中";
                } else if (item.ParseState == Core.Item.EParseState.Done) {
                    State = "完成";
                }
            } else {
                if (item.ParseState == Core.Item.EParseState.Failed) {
                    State = "信息解析失败";
                } else {
                    if (item.DownloadState == Core.Item.EDownloadState.InQueue) {
                        State = "等待下载...";
                    } else if (item.DownloadState == Core.Item.EDownloadState.Handling) {
                        State = "下载中";
                    } else if (item.DownloadState == Core.Item.EDownloadState.Missing) {
                        State = "文件丢失";
                    } else if (item.DownloadState == Core.Item.EDownloadState.Done) {
                        State = "完成";
                    } else if (item.DownloadState == Core.Item.EDownloadState.Failed) {
                        State = item.GetFailMessage();
                    }
                }
            }
            // 加载缩略图
            if (string.IsNullOrEmpty(item.UrlPreview)) {
                PreviewImage = "/img/default-preview.png";
            }
            string? fileName = Core.Helper.FindFileIgnoreExt(Core.Constants.PreviewDirName, item.ItemId);
            if (fileName != null) {
                PreviewImage = Path.GetFullPath(Core.Constants.PreviewDirName + fileName);
            } else {
                PreviewImage = String.Empty;
            }
        }

        private static List<string> _downloadingPreviews = [];
        // 下载缩略图
        public async Task DownloadPreviewImage() {
            if (_downloadingPreviews.Contains(Item.ItemId)) return;
            lock (_downloadingPreviews) {
                _downloadingPreviews.Add(Item.ItemId);
            }
            if (!Directory.Exists(Core.Constants.PreviewDirName)) {
                Directory.CreateDirectory(Core.Constants.PreviewDirName);
            }
            var res = await Core.Helper.DownloadImage(Item.UrlPreview, Core.Constants.PreviewDirName + Item.ItemId);
            if (res != null) {
                PreviewImage = Path.GetFullPath(res);
            }
            lock (_downloadingPreviews) {
                _downloadingPreviews.Remove(Item.ItemId);
            }
        }
    }
}