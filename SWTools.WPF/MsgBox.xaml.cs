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
    /// MsgBox.xaml 的交互逻辑
    /// </summary>
    public partial class MsgBox : Window {
        // 数据
        private string _linkUrl = "";

        public MsgBox(string title, string content, bool isYesNoBox) {
            InitializeComponent();
            Title2.Text = Title = title;
            TextBlock.Text = content;
            if (isYesNoBox) {
                BtnNo.Visibility = Visibility.Visible;
            } else {
                BtnYesText.Text = " 确认";
                BtnNo.Visibility = Visibility.Hidden;
            }
            BtnLink.Visibility = Visibility.Hidden;
        }
        public MsgBox(string title, string content, bool isYesNoBox, string linkText, string linkUrl) {
            InitializeComponent();
            Title2.Text = Title = title;
            TextBlock.Text = content;
            if (isYesNoBox) {
                BtnNo.Visibility = Visibility.Visible;
            } else {
                BtnYesText.Text = " 确认";
                BtnNo.Visibility = Visibility.Hidden;
            }
            BtnLink.Visibility = Visibility.Visible;
            BtnLinkText.Text = ' ' + linkText;
            _linkUrl = linkUrl;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void BtnLink_Click(object sender, RoutedEventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", _linkUrl);
        }
    }
}
