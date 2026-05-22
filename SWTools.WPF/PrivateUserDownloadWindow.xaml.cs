using Microsoft.VisualBasic;
using SWTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;

namespace SWTools.WPF {
    /// <summary>
    /// PrivateUserDownloadWindow.xaml 的交互逻辑
    /// </summary>
    /// 我将json处理逻辑放在前端了，以后会改动成后端的，先这样吧
    public partial class PrivateUserDownloadWindow : Window {
        public PrivateUserDownloadWindow() {
            InitializeComponent();
            if (!Directory.Exists(SWTools.Core.Constants.PrivateDir)) {
                Directory.CreateDirectory(SWTools.Core.Constants.PrivateDir);
            } if (!File.Exists(SWTools.Core.Constants.PrivateAccountFile)) {
                File.Create(SWTools.Core.Constants.PrivateAccountFile).Close();
                File.WriteAllText(SWTools.Core.Constants.PrivateAccountFile, "[]");
            }
            flash();

        }

        private void btn_Download(object sender, RoutedEventArgs e) {
            
        }

        public void flash() {
            UserChoose.Items.Clear();
            string json = File.ReadAllText(SWTools.Core.Constants.PrivateAccountFile);
            if (json == null) {
                MsgBox msg;
                msg = new("错误", "无法读取账户数据。", false) { Owner = this };
                return;

            }
            List<Account>? accountList = JsonSerializer.Deserialize<List<Account>>(json, SWTools.Core.Constants.JsonOptions);
            List<string> nameList = new List<string>();
            if (accountList != null) {
                foreach (Account account in accountList) {
                    string name = account.Name;
                    UserChoose.Items.Add(name);
                }
            } else {
                MsgBox msgBox = new("错误", "无法加载账户数据。", false) { Owner = this };
            }

                return;

        }

        private void BtnAdd_User_Click(object sender, RoutedEventArgs e) {
            string passwd = Steam_Passwd.Text;
            string user = Steam_user.Text;
            if (string.IsNullOrWhiteSpace(passwd) || string.IsNullOrWhiteSpace(user)) {
                MsgBox msgBox = new("输入错误", "用户名和密码不能为空。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            List<PrivAccount> list;
            string json = File.ReadAllText(SWTools.Core.Constants.PrivateAccountFile);
            if (json != null) {
                list = JsonSerializer.Deserialize<List<PrivAccount>>(json, SWTools.Core.Constants.JsonOptions) ?? new List<PrivAccount>();
                list.Add(new PrivAccount { Name = user, Password = passwd });
                string newjson = JsonSerializer.Serialize(list, SWTools.Core.Constants.JsonOptions);
                File.WriteAllText(SWTools.Core.Constants.PrivateAccountFile, newjson);
                flash();

            } else {
                list = new List<PrivAccount>();
                list.Add(new PrivAccount { Name = user, Password = passwd });
                string newjson = JsonSerializer.Serialize(list, SWTools.Core.Constants.JsonOptions);
                File.WriteAllText(SWTools.Core.Constants.PrivateAccountFile, newjson);
                flash();
            }

        }

    }
}
