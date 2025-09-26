using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SWTools.WPF {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        // 非模态子窗口 (希望不重复创建)
        private AddTaskWindow? _addTaskWindow;
        private LogWindow? _logWindow;

        // ViewModel 访问点
        public ViewModel.MainWindow ViewModel {
            get => (ViewModel.MainWindow)DataContext;
            set { DataContext = value; }
        }

        public MainWindow() {
            InitializeComponent();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e) {
            if (_addTaskWindow == null) {
                _addTaskWindow = new AddTaskWindow() { Owner = this };
                _addTaskWindow.Closed += (s, args) => {
                    _addTaskWindow = null;
                    Activate(); // 解决一个奇怪的 bug
                };
            }
            _addTaskWindow.Show();
            _addTaskWindow.Activate();
            if (_addTaskWindow.WindowState == WindowState.Minimized) {
                _addTaskWindow.WindowState = WindowState.Normal;
            }
        }

        private void BtnMore_Click(object sender, RoutedEventArgs e) {
            MoreWindow moreWindow = new() { Owner = this };
            moreWindow.ShowDialog();
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e) {
            if (_logWindow == null) {
                _logWindow = new LogWindow() { Owner = this };
                _logWindow.Closed += (s, args) => {
                    _logWindow = null;
                    Activate(); // 解决一个奇怪的 bug
                };
            }
            _logWindow.Show();
            _logWindow.Activate();
            if (_logWindow.WindowState == WindowState.Minimized) {
                _logWindow.WindowState = WindowState.Normal;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            // 检查失败和失踪的
            List<string> fmItems = [];
            foreach (var item in ViewModel.Items) {
                if (item.DownloadState == Core.Item.EDownloadState.Missing ||
                    item.DownloadState == Core.Item.EDownloadState.Failed) {
                    fmItems.Add(item.ItemId);
                }
            }
            if (fmItems.Count > 0) {
                MsgBox msgBox = new("操作确认", "下载列表中包含下载失败或文件丢失的物品，\n您想要重新下载吗？", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) {
                    foreach (var item in fmItems) {
                        ViewModel.Items[ViewModel.Items.FindIndex(item)].DownloadState = Core.Item.EDownloadState.InQueue;
                    }
                }
            }
            // 检查队列中是否有物品
            bool hasInQueue = false;
            foreach (var item in ViewModel.Items) {
                if (item.DownloadState == Core.Item.EDownloadState.InQueue) {
                    hasInQueue = true;
                    break;
                }
            }
            if (!hasInQueue) {
                MsgBox msgBox = new("没有要下载的物品", "下载队列为空。请单击 “添加下载任务” 添加要下载的项。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            // 开始
            _ = ViewModel.StartDownload();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            ViewModel.IsDownloading = false;
            ViewModel.IsBtnStopEnable = false;
            ViewModel.StatusText += "（完成此项后暂停）";
        }

        private void BtnRemoveAll_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("操作确认", "确认要清空下载列表吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                ViewModel.Items.Clear();
            }
        }

        private void BtnRemoveFM_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("操作确认", "确认要移除下载失败和文件丢失的物品吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                List<Core.Item> items = [];
                foreach (var item in ViewModel.Items) {
                    if (item.DownloadState == Core.Item.EDownloadState.Failed ||
                        item.DownloadState == Core.Item.EDownloadState.Missing) {
                        items.Add(item);
                    }
                }
                foreach (var item in items) {
                    ViewModel.Items.Remove(item);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            ViewModel.CleanUp();
        }

        private void MenuRemove_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            if (item.Item.DownloadState == Core.Item.EDownloadState.Handling) {
                MsgBox msgBox = new("操作失败", "请等待当前物品下载完成。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            if (item.Item.DownloadState == Core.Item.EDownloadState.Done) {
                MsgBox msgBox = new("操作确认", "确认要从列表中移除吗？\n" +
                    "这样做不会删除文件，已下载的物品将占用空间，直到您手动删除。", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) {
                    ViewModel.Items.Remove(item.Item);
                }
            } else {
                ViewModel.Items.Remove(item.Item);
            }
        }

        private void MenuOpenFolder_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            if (item.Item.DownloadState != Core.Item.EDownloadState.Done) {
                MsgBox msgBox = new("操作失败", "物品尚未完成下载。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            System.Diagnostics.Process.Start("explorer.exe",
                Path.GetFullPath(item.Item.GetDownloadPath()));
        }

        private void MenuRetry_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            if (item.Item.DownloadState == Core.Item.EDownloadState.Handling) {
                MsgBox msgBox = new("操作失败", "请等待当前物品下载完成。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            item.Item.DownloadState = Core.Item.EDownloadState.InQueue;
            ViewModel.UpdateDisplay();
            if (!ViewModel.IsDownloading) { // 启动下载
                _ = ViewModel.DownloadOne(item.Item.ItemId);
            }
        }

        private void MenuCopy_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            Clipboard.SetDataObject(item.Item.ItemId);
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            MsgBox msgBox;
            if (item.Item.DownloadState == Core.Item.EDownloadState.Handling) {
                msgBox = new("操作失败", "请等待当前物品下载完成。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            if (item.Item.DownloadState != Core.Item.EDownloadState.Done) {
                msgBox = new("操作失败", "物品尚未完成下载。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            msgBox = new("操作确认", "确认要删除此物品的文件吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                try {
                    Directory.Delete(item.Item.GetDownloadPath(), true);
                }
                catch (Exception) {
                    msgBox = new("操作失败", "未能成功删除文件，您可以尝试自行删除", true) { Owner = this };
                    msgBox.ShowDialog();
                }
                ViewModel.Items.Remove(item.Item);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // 拉取最新信息
            if (!Core.ConfigManager.Config.NoAutoFetch) {
                ViewModel.FetchRepo();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (ViewModel.IsIndeterminate) {
                MsgBox msgBox = new("请勿关闭此窗口", "请等待当前任务完成。此时关闭窗口可能引发未知的问题。", false) { Owner = this };
                msgBox.ShowDialog();
                e.Cancel = true;
            }
        }

        private void MenuLink_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            System.Diagnostics.Process.Start("explorer.exe",
                $"https://www.steamcommunity.com/sharedfiles/filedetails/{item.Item.ItemId}");
        }
    }
}