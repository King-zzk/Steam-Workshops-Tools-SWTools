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
using Path = System.IO.Path;

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

    public void judgment(string GameName, string modPath) {

            if (GameName == "Hearts of Iron IV") {
                string prgammerpath = AppDomain.CurrentDomain.BaseDirectory;
                string tmpPath = prgammerpath + "tmp\\";
                if (!Directory.Exists(tmpPath)) {
                    Directory.CreateDirectory(tmpPath);
                }
                string sourceFile = modPath + "/descriptor.mod";
                if (!File.Exists(sourceFile)) {
                    MessageBox.Show("模组路径下没有找到descriptor.mod文件，无法继续！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                } else {
                    int len = modPath.Length;
                    string subModPath = tmpPath + modPath.Substring(len - 7, 7) + ".mod";
                    File.Copy(sourceFile, subModPath, true);
                    string addText = "path=" + modPath;
                    File.AppendAllText(subModPath, Environment.NewLine + addText);
                    Process.Start("explorer.exe", tmpPath);
                    MsgBox msgBox = new MsgBox("成功", "请进入钢铁雄心4启动器创建模组，找到游戏mod文件夹，将这个.mod内容替换到刚刚创建的.mod的配置" +
                    "文件即可完成！", false) { Owner = this };
                    msgBox.ShowDialog();
                }
            }
        }


        private void install_Click(object sender, RoutedEventArgs e) {
            string? selectedText = GameItems.SelectedItem?.ToString();
            string? modPath = ModPath.Text;
            MsgBox msgBox;
            if (string.IsNullOrEmpty(selectedText)) {
                msgBox = new("错误", "您没有选中游戏！", false) { Owner = this };
                msgBox.ShowDialog();
            } else if (string.IsNullOrEmpty(modPath)) {
                msgBox = new("错误", "您没有输入模组路径！", false) { Owner = this };
                msgBox.ShowDialog();
            } else if (!Directory.Exists(modPath)) {
                msgBox = new("错误", "您模组路径不存在！", false) { Owner = this };
                msgBox.ShowDialog();
            } else {
                judgment(selectedText, modPath);
            }
        }
        }
    }
