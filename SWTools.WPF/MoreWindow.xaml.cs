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

namespace SWTools.WPF {
    /// <summary>
    /// MoreWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MoreWindow : Window {
        public MoreWindow() {
            InitializeComponent();
            VersionText.Text = $"这是 Steam Workshop Tools v{SWTools.Core.Helper.GetVersionStr()}。";
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe",
                "https://github.com/King-zzk/Steam-Workshops-Tools-SWTools");
        }
    }
}
