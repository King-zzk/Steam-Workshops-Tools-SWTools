using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SWTools.ViewModel {
    /// <summary>
    /// MainWindow 下载服务相关
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged {
        // 下载队列（工作队列）
        private List<string> downloadQueue = [];
        public int QueueSize { get { return downloadQueue.Count; } }
        // 是否正在执行下载
        public bool IsDownloading { get; private set; } = false;
        public bool Stopping { get; private set; } = false;

        /* 对外的 CRUD */

        // 添加物品 (自动过滤信息不完整的)
        public void Append(Core.ItemList comingItems, bool overriding) {
            foreach (var item in comingItems) {
                if (items.Contains(item.ItemId)) {
                    if (!overriding) continue;
                    items.Remove(item.ItemId);
                }
                if (item.AppId == 0) continue;
                items.Add(item);
            }
            UpdateDisplay();
        }

        // 移除指定物品
        public bool Remove(Core.Item item) {
            lock (items) {
                if (items.Find(item.ItemId)?.DownloadState == Core.Item.EDownloadState.Handling) {
                    return false;
                }
                items.Remove(item.ItemId);
                downloadQueue.Remove(item.ItemId);
                return true;
            }
        }

        // 清空全部物品
        public bool Clear() {
            if (IsDownloading) return false;
            items.Clear();
            downloadQueue.Clear();
            return true;
        }

        // 检查是否有与给定列表 重复的物品
        public bool HasDuplicated(Core.ItemList comingItems) {
            foreach (var item in comingItems) {
                if (items.Contains(item.ItemId)) {
                    return true;
                }
            }
            return false;
        }

        // 检查是否有失败或失踪的物品
        public bool HasFailedOrMissing() {
            foreach (var item in items) {
                if (item.DownloadState == Core.Item.EDownloadState.Missing ||
                    item.DownloadState == Core.Item.EDownloadState.Failed) {
                    return true;
                }
            }
            return false;
        }

        // 移除失败或失踪的物品
        public void RemoveFailedOrMissing() {
            var fmItems = from item in items
                      where item.DownloadState == Core.Item.EDownloadState.Missing || item.DownloadState == Core.Item.EDownloadState.Failed
                      select item;
            if (fmItems == null) return;
            foreach (var item in fmItems) {
                if (items.Contains(item.ItemId)) items.Remove(item.ItemId);
                if (downloadQueue.Contains(item.ItemId)) downloadQueue.Remove(item.ItemId);
            }
            UpdateDisplay();
        }

        // 将总列表中的指定物品加入下载队列
        public bool QueueAppend(Core.Item item) {
            if (!items.Contains(item.ItemId)) return false;
            downloadQueue.Add(item.ItemId);
            UpdateDisplay();
            return true;
        }

        // 将总列表所有物品加入下载队列
        public void QueueAppendAll(bool includingFailedOrMissing) {
            foreach (var item in items) {
                if (item.DownloadState == Core.Item.EDownloadState.Pending) {
                    downloadQueue.Add(item.ItemId);
                } else if (item.DownloadState == Core.Item.EDownloadState.Missing ||
                         item.DownloadState == Core.Item.EDownloadState.Failed) {
                    if (!includingFailedOrMissing) continue;
                    downloadQueue.Add(item.ItemId);
                }
            }
            UpdateDisplay();
        }

        /* 下载逻辑 */
        // 切换状态
        private void ToggleStatus(bool isDownloading) {
            IsDownloading = isDownloading;

            IsBtnAddTaskEnable = !isDownloading;
            IsBtnStartEnable = !isDownloading;
            IsBtnRemoveAllEnable = !isDownloading;

            IsBtnStopEnable = isDownloading;
            IsIndeterminate = isDownloading;
        }

        // 启动下载
        public async Task QueueLaunch() {
            // 开始
            ToggleStatus(true);

            // 准备 Steamcmd
            StatusText = "正在准备 Steamcmd，请耐心等待（在日志中查看详细信息）";
            await Core.Helper.Steamcmd.Setup();
            // 开始下载
            bool isEarlyStopped = false;
            while (downloadQueue.Count > 0 && !Stopping) {
                var item = downloadQueue[0];
                downloadQueue.RemoveAt(0);
                var displayItem = (from dItem in DisplayItems
                                   where dItem.Item.ItemId == item
                                   select dItem).First();
                StatusText = $"正在下载「{displayItem.ItemName}」";
                Core.LogManager.Log.Information("Downloading item {itemId}", displayItem.Item.ItemId);
                displayItem.Item.PropertyChanged += (s, e) => {
                    UpdateDisplay();
                };
                await displayItem.Item.Download();
                if (displayItem.Item.DownloadState == Core.Item.EDownloadState.Failed &&
                    displayItem.Item.FailReason == Core.Item.EFailReason.NoConnection) {
                    StatusText = "已停止";
                    isEarlyStopped = true;
                    break;
                }
            }

            // 结束
            if (!isEarlyStopped) {
                StatusText = "已完成";
            }
            ToggleStatus(false);
        }

        public void Stop() {
            Stopping = true;
        }
    }
}
