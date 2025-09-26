using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SWTools.WPF {
    /// <summary>
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window {
        public LogWindow() {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe",
                Path.GetFullPath(Core.Constants.LogDir));
        }

        private void LogText_TextChanged(object sender, TextChangedEventArgs e) {
            LogText.ScrollToEnd();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            LogText.ScrollToEnd();
        }

        private void BtnOpenSteamcmdFolder_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe",
                Path.GetFullPath(Core.Constants.SteamcmdDir + "logs/"));
        }
    }
}
