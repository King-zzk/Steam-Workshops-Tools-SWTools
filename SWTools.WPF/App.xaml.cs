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
            Core.Helper.Main.SetupAll();
        }

        // 程序退出时...
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

            // 关闭 Core
            Core.Helper.Main.CleanupAll();

            // 强制退出所有线程
            Environment.Exit(0);
        }
    }
}
