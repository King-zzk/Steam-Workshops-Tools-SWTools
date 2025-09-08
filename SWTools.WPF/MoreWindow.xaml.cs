using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MoreWindow : Window
    {
        public MoreWindow()
        {
            InitializeComponent();
            VersionText.Text = $"这是 Steam Workshop Tools v{SWTools.Core.Helper.GetVersionStr()}。";

            if (File.Exists("./background.dat"))
            {
                // 读取 background.dat 文件内容作为背景图片路径
                string backgroundfile;
                using (FileStream F = new FileStream("./background.dat", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[F.Length];
                    F.ReadExactly(buffer);
                    backgroundfile = Encoding.UTF8.GetString(buffer);
                    F.Close();
                }
                if (backgroundfile != null)
                {
                    this.Background = new ImageBrush(new BitmapImage(new Uri(backgroundfile, UriKind.Absolute)));
                }
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnGithub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe",
                "https://github.com/King-zzk/Steam-Workshops-Tools-SWTools");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // 获取文本框内容
            string? backgroundfile = backgroundtextbox?.Text;

            if (string.IsNullOrWhiteSpace(backgroundfile))
            {
                MessageBox.Show("请输入文件地址！");
                return;
            }

            if (!File.Exists(backgroundfile))
            {
                MessageBox.Show("文件不存在，请检查路径是否正确！");
                return;
            }

            try
            {
                // 存储文件地址 
                FileStream F = new FileStream("background.dat",
                    FileMode.OpenOrCreate, FileAccess.ReadWrite);
                F.Write(Encoding.UTF8.GetBytes(backgroundfile));
                F.Close();
                MessageBox.Show("设置背景成功，请重启软件以应用新背景！","提示",MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置背景失败: {ex.Message}");
            }
        }
    }
}
