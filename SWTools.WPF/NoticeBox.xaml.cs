using SWTools.Core;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SWTools.WPF {
    /// <summary>
    /// NoticeBox.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeBox : Window {
        public NoticeBox(string noticeMd) {
            InitializeComponent();
            WebViewer.NavigateToString(Helper.MdToHtml(noticeMd));
            WebViewer.Navigating += (s, e) => {
                e.Cancel = true;
                System.Diagnostics.Process.Start("explorer.exe", e.Uri.ToString());
            };
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
