using System.Configuration;
using System.Data;
using System.Windows;

namespace SWTools.WPF {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            // 启动 Core
            Core.Helper.SetupAll();
        }

        // 程序退出时...
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            // 关闭 Core
            Core.Helper.CleanupAll();
            Environment.Exit(0); // 强制退出所有线程
        }
    }
}
