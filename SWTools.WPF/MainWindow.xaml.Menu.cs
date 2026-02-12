using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SWTools.WPF {
    /// <summary>
    /// MainWindow.xaml 的菜单交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        /* 右键菜单响应程序 Menu- */

        private static ViewModel.DisplayItem? MenuItemCheck(object sender) {
            if (sender is not MenuItem menuItem) return null;
            if (menuItem.Parent is not ContextMenu contextMenu) return null;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return null;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return null;
            return item;
        }

        private void Menu_Opened(object sender, RoutedEventArgs e) {
            if (sender is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            if (item == null) return;

            (contextMenu.Items[0] as MenuItem)?.IsEnabled = !item.IsHandling; // 移除
            (contextMenu.Items[3] as MenuItem)?.IsEnabled = !item.IsHandling; // 重试

            (contextMenu.Items[1] as MenuItem)?.IsEnabled = item.IsDownloaded; // 打开文件夹
            (contextMenu.Items[7] as MenuItem)?.IsEnabled = item.IsDownloaded; // 删除
            (contextMenu.Items[7] as MenuItem)?.Foreground = item.IsDownloaded ? 
                Brushes.Firebrick : (contextMenu.Items[1] as MenuItem)?.Foreground;
        }

        private void MenuRemove_Click(object sender, RoutedEventArgs e) {
            var item = MenuItemCheck(sender);
            if (item == null) return;
            if (item.Item.DownloadState == Core.Item.EDownloadState.Done) {
                MsgBox msgBox = new("操作确认", "确认要从列表中移除吗？\n" +
                    "此操作不会删除磁盘上的文件，您可能需要手动删除。", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) {
                    ViewModel.Remove(item.Item);
                }
            } else {
                ViewModel.Remove(item.Item);
            }
        }

        private void MenuOpenFolder_Click(object sender, RoutedEventArgs e) {
            var item = MenuItemCheck(sender);
            if (item == null) return;
            System.Diagnostics.Process.Start("explorer.exe",
                Path.GetFullPath(item.Item.GetDownloadPath()));
        }

        private void MenuRetry_Click(object sender, RoutedEventArgs e) {
            var item = MenuItemCheck(sender);
            if (item == null) return;
            item.Item.DownloadState = Core.Item.EDownloadState.Pending;
            ViewModel.QueueAppend(item.Item);
            if (!ViewModel.IsDownloading) { // 启动下载
                _ = ViewModel.QueueLaunch();
            }
        }

        private void MenuCopy_Click(object sender, RoutedEventArgs e) {
            var item = MenuItemCheck(sender);
            if (item == null) return;
            Clipboard.SetDataObject(item.Item.ItemId);
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e) {
            var item = MenuItemCheck(sender);
            if (item == null) return;
            MsgBox msgBox;
            msgBox = new("操作确认", "确认要删除此物品的文件吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                try {
                    Directory.Delete(item.Item.GetDownloadPath(), true);
                    ViewModel.Remove(item.Item);
                }
                catch (Exception) {
                    msgBox = new("操作失败", "未能成功删除文件，您可以尝试自行删除", true) { Owner = this };
                    msgBox.ShowDialog();
                }
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