using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace SWTools.WPF {
    /// <summary>
    /// MoreWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MoreWindow : Window {
        // ViewModel 访问点
        public ViewModel.MoreWindow ViewModel {
            get { return DataContext as ViewModel.MoreWindow; }
            set { DataContext = value; }
        }

        public MoreWindow() {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe",
                "https://github.com/King-zzk/Steam-Workshops-Tools-SWTools");
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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", e.Uri.ToString());
        }
    }
}
