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
using System.Windows.Shapes;

namespace SWTools.WPF {
    /// <summary>
    /// ModinstallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModinstallWindow : Window {
        

        public ModinstallWindow() {
            InitializeComponent();
        }

        private void install_Click(object sender, RoutedEventArgs e) {
            string? selectText = GameItems.SelectedItem as string;
            string? modPath = ModPath.Text;
            string? installPath = GamesinstallModPath.Text;
            if (selectText == string.Empty) {
                MsgBox msgBox = new("错误", "您没有选中游戏！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (modPath == string.Empty) {
                MsgBox msgBox = new("错误", "您没有输入模组路径！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (installPath == string.Empty) {
                MsgBox msgBox = new("错误", "您没有输入游戏安装路径！", true) { Owner = this };
                msgBox.ShowDialog();
            }
            }
        }
}
