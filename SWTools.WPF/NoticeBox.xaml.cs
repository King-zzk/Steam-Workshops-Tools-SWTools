using System.Windows;

namespace SWTools.WPF {
    /// <summary>
    /// NoticeBox.xaml 的交互逻辑
    /// </summary>
    public partial class NoticeBox : Window {
        public NoticeBox(string noticeMd) {
            InitializeComponent();
            MdViewer.Content = noticeMd;
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            MdViewer.Dispose();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
