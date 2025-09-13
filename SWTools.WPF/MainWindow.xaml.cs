using System.ComponentModel;
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
        // 非模态子窗口 (希望不重复创建)
        private AddTaskWindow? _addTaskWindow;
        private LogWindow? _logWindow;

        // ViewModel 访问点
        public ViewModel.MainWindow ViewModel {
            get { return DataContext as ViewModel.MainWindow; }
            set { DataContext = value; }
        }

        public MainWindow() {
            // 初始化
            InitializeComponent();
            // 添加调试信息
            //ViewModel.AddDebugData();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e) {
            if (_addTaskWindow == null) {
                _addTaskWindow = new AddTaskWindow() { Owner = this };
                _addTaskWindow.Closed += (s, args) => {
                    _addTaskWindow = null;
                    Activate(); // 解决一个奇怪的 bug
                };
            }
            _addTaskWindow.Show();
            _addTaskWindow.Activate();
            if (_addTaskWindow.WindowState == WindowState.Minimized) {
                _addTaskWindow.WindowState = WindowState.Normal;
            }
        }

        private void BtnMore_Click(object sender, RoutedEventArgs e) {
            MoreWindow moreWindow = new() { Owner = this };
            moreWindow.ShowDialog();
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e) {
            if (_logWindow == null) {
                _logWindow = new LogWindow() { Owner = this };
                _logWindow.Closed += (s, args) => {
                    _logWindow = null;
                    Activate(); // 解决一个奇怪的 bug
                };
            }
            _logWindow.Show();
            _logWindow.Activate();
            if (_logWindow.WindowState == WindowState.Minimized) {
                _logWindow.WindowState = WindowState.Normal;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
        }
    }
}