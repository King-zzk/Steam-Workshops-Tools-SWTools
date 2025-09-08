using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWTools.WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
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

        private void BtnAddTask_Click(object sender, RoutedEventArgs e) {
            AddTaskWindow addTaskWindow = new();
            addTaskWindow.ShowDialog();
        }

        private void BtnMore_Click(object sender, RoutedEventArgs e) {
            MoreWindow moreWindow = new MoreWindow();
            moreWindow.ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}