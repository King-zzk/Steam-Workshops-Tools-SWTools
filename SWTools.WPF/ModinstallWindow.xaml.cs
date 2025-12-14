using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// ModinstallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModInstallWindow : Window {


        public ModInstallWindow() {
            InitializeComponent();
        }

        private void install_Click(object sender, RoutedEventArgs e) {
            string? selectText = GameItems.SelectedItem as string;
            string? modPath = ModPath.Text;
            string? installPath = GamesinstallModPath.Text;
            if (selectText == string.Empty) {
                MsgBox msgBox = new("错误", "您没有选中游戏！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (modPath == string.Empty) {
                MsgBox msgBox = new("错误", "您没有输入模组路径！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (installPath == string.Empty) {
                MsgBox msgBox = new("错误", "您没有输入游戏安装路径！", true) { Owner = this };
                msgBox.ShowDialog();
            } else {
                if (selectText == "钢铁雄心4") {
                    ModInstall_gtxx(modPath, installPath);
                } else {
                    MsgBox msgBox = new("错误", "没有获取游戏信息？？？", true) { Owner = this };
                    msgBox.ShowDialog();
                }
            }
        }
        /* 未完工 的钢铁雄心4模组安装功能 参考:https://www.bilibili.com/opus/602208299562833051 */
        public void ModInstall_gtxx(String mod_path, String Game_path) {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string command = "xcopy" + mod_path + Game_path + " /s /e /y";
            p.StandardInput.WriteLine(command + "&exit");
            p.StandardInput.AutoFlush = true;

            // 获取cmd窗口的输出信息

            string output = p.StandardOutput.ReadToEnd();

            // 等待cmd窗口执行完毕，退出cmd窗口
            p.WaitForExit();
            p.Close();

            MsgBox msgBox = new("返回结果", output, true) { Owner = this };
            msgBox.ShowDialog();

            

            }
        }
    }
