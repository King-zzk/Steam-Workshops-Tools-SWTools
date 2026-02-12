using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SWTools.WPF {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        // 非模态子窗口 (希望不重复创建)
        private AddTaskWindow? _addTaskWindow;
        private LogWindow? _logWindow;

        // ViewModel 访问点
        public ViewModel.MainWindow ViewModel {
            get => (ViewModel.MainWindow)DataContext;
            set { DataContext = value; }
        }

        public MainWindow() {
            InitializeComponent();
        }

        /* 按钮响应程序 Btn- */

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
            // 检查失败和失踪的
            bool includingFailedOrMissing = false;
            if (ViewModel.HasFailedOrMissing()) {
                MsgBox msgBox = new("操作确认", "下载列表中包含下载失败或文件丢失的物品，\n您想要重新下载吗？", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) includingFailedOrMissing = true;
            }
            // 添加所有物品到下载队列
            ViewModel.QueueAppendAll(includingFailedOrMissing);
            // 检查队列中是否有物品
            if (ViewModel.QueueSize == 0) {
                MsgBox msgBox = new("没有要下载的物品", "下载队列为空。请单击 “添加下载任务” 添加要下载的项。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            // 开始
            _ = ViewModel.QueueLaunch();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            ViewModel.Stop();
            ViewModel.IsBtnStopEnable = false;
            ViewModel.StatusText += "（完成此项后暂停）";
        }

        private void BtnRemoveAll_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("操作确认", "确认要清空下载列表吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                ViewModel.Clear();
            }
        }

        private void BtnRemoveFM_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("操作确认", "确认要移除下载失败和文件丢失的物品吗？\n此操作不可撤销。", true) { Owner = this };
            bool? res = msgBox.ShowDialog();
            if (res == true) {
                ViewModel.RemoveFailedOrMissing();
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            ViewModel.CleanUp();
        }

        /* 窗口响应程序 Window- */

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            // 拉取最新信息
            if (!Core.ConfigManager.Config.NoAutoFetch) {
                string? notice = await ViewModel.FetchRepo();
                if (string.IsNullOrEmpty(notice)) return;
                NoticeBox msgBox = new(notice) { Owner = this };
                msgBox.ShowDialog();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (ViewModel.IsIndeterminate) {
                MsgBox msgBox = new("请勿关闭此窗口", "请等待当前任务完成。此时关闭窗口可能引发未知的问题。\n\n如果您坚持要关闭窗口，请单击 “否”。", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) {
                    e.Cancel = true;
                }
            }
        }
    }
}