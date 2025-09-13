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
            Core.ConfigManager.Setup();
            Core.LogManager.Setup();
        }

        // 程序退出时...
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            Core.ConfigManager.Save("Exit"); // 保存配置
            Environment.Exit(0); // 强制退出所有线程
        }
    }
}
