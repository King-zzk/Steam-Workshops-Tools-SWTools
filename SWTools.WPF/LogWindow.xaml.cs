using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

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
                Path.GetFullPath(Core.Constants.LogDirName));
        }
    }
}
