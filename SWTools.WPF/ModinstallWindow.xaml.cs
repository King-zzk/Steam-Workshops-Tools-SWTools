using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
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
            GameItems.Items.Add("Hearts of Iron IV");
        }

        // 未完工

        public void judgment(string GameName, string installPath, string modPath) {
            if (GameName == "Hearts of Iron IV") {
                string sourceFile = modPath + "\\descriptor.mod";
                int len = modPath.Length;
                string subModPath = installPath  + "\\"  + modPath.Substring(len - 7, 7) + ".mod";
                File.Copy(sourceFile, subModPath, true);
                string addText = "path=" + modPath;
                File.AppendAllText(subModPath, Environment.NewLine + addText);
            }
        }


        private void install_Click(object sender, RoutedEventArgs e) {
            string? selectedText = GameItems.SelectedItem?.ToString();
            string? modPath = ModPath.Text;
            string? installPath = GamesinstallModPath.Text;
            MsgBox msgBox = new MsgBox("调试", $"选择的游戏:{selectedText}\n模组路径:{modPath}\n游戏安装路径:{installPath}", true) { Owner = this };
            msgBox.ShowDialog();
            if (string.IsNullOrEmpty(selectedText)) {
                msgBox = new("错误", "您没有选中游戏！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (string.IsNullOrEmpty(modPath)) {
                msgBox = new("错误", "您没有输入模组路径！", true) { Owner = this };
                msgBox.ShowDialog();
            } else if (string.IsNullOrEmpty(installPath)) {
                msgBox = new("错误", "您没有输入游戏安装路径！", true) { Owner = this };
                msgBox.ShowDialog();
            } else {
                judgment(selectedText, installPath, modPath);
            }
        }
        }
    }
