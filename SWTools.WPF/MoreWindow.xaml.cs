using Semver;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace SWTools.WPF {
    /// <summary>
    /// MoreWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MoreWindow : Window {
        // ViewModel 访问点
        public ViewModel.MoreWindow ViewModel {
            get => (ViewModel.MoreWindow)DataContext;
            set { DataContext = value; }
        }

        public MoreWindow() {
            InitializeComponent();
            LicenseText.Text = Helper.GetEmbeddedResource("SWTools.WPF.LICENSE.txt");
            MdViewer.Content = Helper.GetEmbeddedResource("SWTools.WPF.THIRD-PARTY-NOTICE.md");
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            MdViewer.Dispose();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe",
                Core.Constants.UrlRepo);
        }

        private void BtnClearCache_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox0 = new("操作确认", "确认要清空缓存吗？\n", true) { Owner = this };
            if (msgBox0.ShowDialog() == true) {
                Core.Helper.Main.ClearAllCache();
                MsgBox msgBox = new("清理完成", "已删除缓存（程序正在引用的缓存除外）。", false) { Owner = this };
                msgBox.ShowDialog();
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox0 = new("操作确认", "确认要重置所有设置吗？\n", true) { Owner = this };
            if (msgBox0.ShowDialog() == true) {
                ViewModel.Config = new();
            }
        }

        private void BtnOpenDownloadFolder_Click(object sender, RoutedEventArgs e) {
            var path = Core.Constants.SteamcmdDir + "steamapps/workshop/content/";
            System.Diagnostics.Process.Start("explorer.exe",
                Path.GetFullPath(path));
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", e.Uri.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // 提示新版本
            if (!SWTools.ViewModel.MoreWindow.HasHintedLatestVersion) {
                SWTools.ViewModel.MoreWindow.HasHintedLatestVersion = true;
                var info = Core.Helper.Main.ReadLatestInfo();
                if (info == null) return;
                if (info.Release != null &&
                    SemVersion.Parse(info.Release).CompareSortOrderTo(Core.Constants.Version) > 0) {
                    MsgBox msgBox = new("发现新版本", $"检测到新的发行版：{info.Release}\n（当前版本：{Core.Constants.Version}）\n\n" +
                        $"您可以在下方链接获取新版本。", false,
                        "查看 Release 页面", Core.Constants.UrlRelease) { Owner = this };
                    msgBox.ShowDialog();
                } else if (info.PreRelease != null &&
                    SemVersion.Parse(info.PreRelease).CompareSortOrderTo(Core.Constants.Version) > 0) {
                    MsgBox msgBox = new("发现新的预览版本", $"检测到新的预发行版：{info.Release}\n（当前版本：{Core.Constants.Version}）\n\n" +
                        $"您可以在下方链接获取新版本。", false,
                        "查看 Release 页面", Core.Constants.UrlRelease) { Owner = this };
                    msgBox.ShowDialog();
                }
            }
        }

        private void BtnUninstallSteamcmd_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("操作确认", "确定要卸载 Steamcmd 吗？卸载后可能导致下面的后果：\n\n" +
                "1. 如果您有存放在 Steamcmd 目录下的物品，这些物品将会被删除。您可以点击 “打开总下载目录” 来确认；\n" +
                "2. 下次下载物品时，将重新安装 Steamcmd，下载用时将增加。", true);
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                try {
                    Directory.Delete(Core.Constants.SteamcmdDir, true);
                    Core.LogManager.Log.Information("Successfully deleted steamcmd");
                    msgBox = new("操作成功", $"成功卸载了 Steamcmd。", false);
                    msgBox.ShowDialog();
                } catch (Exception ex) {
                    Core.LogManager.Log.Error("Failed to delete steamcmd: {Exception}", ex);
                    msgBox = new("操作失败", $"无法删除 Steamcmd 所在文件夹。您可以尝试自行删除程序目录下的 {Core.Constants.SteamcmdDir} 目录，" +
                        $"或检查程序日志。", false);
                    msgBox.ShowDialog();
                }
            }
        }
    }
}
