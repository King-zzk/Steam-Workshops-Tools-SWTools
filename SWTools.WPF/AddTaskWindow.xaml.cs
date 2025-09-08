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
    /// AddTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddTaskWindow : Window {
        public AddTaskWindow() {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BtnHelpItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("物品id就是你要下载的MOD，你要下载的物品ID在MOD的网址上/?id=XXXXXXXX，XXXXXXXX就是id编号","什么是物品ID？",MessageBoxButton.OK);
        }

        private void BtnHelpApp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("APPID就是你要下载的MOD所属游戏或工具，你要下载的物品的所属游戏或工具Steam购买网址,比如：https://store.steampowered.com/app/XXXXXX/GAME/,这个XXXXXX就是APP的ID！", "什么是APPID？", MessageBoxButton.OK);
        }
    }
}
