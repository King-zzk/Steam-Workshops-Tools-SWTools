using SWTools.Core;
using SWTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
    /// ChooseSteamUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseSteamUserWindow : Window {
        public ChooseSteamUserWindow() {
            InitializeComponent();
        }
        public static class CmdHelper {
            /// <summary>
            /// 执行 CMD 命令，并返回输出结果
            /// </summary>
            public static string RunCommand(string command) {
                var process = new Process();

                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;       
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                process.Close();

                return string.IsNullOrEmpty(error) ? output : $"{output}\n错误：{error}";
            }
        }

        // 为 RunCommand(string command) 提供方法主体，调用 CmdHelper.RunCommand
        public static string RunCommand(string command) {
            return CmdHelper.RunCommand(command);
        }

        private void Btn_Start(object sender, RoutedEventArgs e) {
            MsgBox msgBox;
            string accountName = user_name.Text;
            string accountPasswd = user_passwd.Text;
            string workshopId = workshop_id.Text;
            string[] linesArray = Regex.Split(workshopId, @"\r?\n|\r");
            if (string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(accountPasswd) || string.IsNullOrEmpty(workshopId)) {
                msgBox = new("输入无效", "请输入有效的账户名，密码和创意工坊编号。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }

            if (!File.Exists("./steamcmd/Steamcmd.exe")) {
                msgBox = new("SteamCMD 未找到", "请使用一次普通下载再使用", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            } else {
                msgBox = new("提示", "您账号可能需要验证码，如果cmd中提示那请输入验证码（你邮箱里的验证码）", false) { Owner = this };
                msgBox.ShowDialog();
                msgBox = new("提示", "可以打开日志获取详细信息", false) { Owner = this };
                msgBox.ShowDialog();
                for (int i = 0; i < linesArray.Length; i++) {
                    tips.Text = "状态：正在下载" + linesArray[i];
                    string command = $"\"{System.IO.Path.Combine("\"steamcmd/steamcmd.exe\"")}\" +login {accountName} {accountPasswd} +workshop_download_item 107410 {linesArray[i]} +quit && pause";
                    LogManager.Log.Information("Command being executed: {Command}", command);
                    string result = RunCommand(command);
                    LogManager.Log.Information("Command output: {Result}", result);
                    // 后续可以调用API获取名字和图片仍进json文件（未完工）
                }
            }

        }

        private void Btn_log(object sender, RoutedEventArgs e) {
            LogWindow win = new();
            win.Show();
        }
    }
}
