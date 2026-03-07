using SWTools.Core;
using SWTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
            btn_flash_click(null, null);
        }
        private void btn_user(object sender, RoutedEventArgs e) {
            List<PriAccount> priAccounts = new List<PriAccount>()
            {
            new PriAccount
                {
                    Name_pri = steam_user_name.Text,
                    Password_pri = steam_user_passwd.Text
                }
            };
            string jsonContent = JsonSerializer.Serialize(priAccounts, Constants.JsonOptions);
            File.WriteAllText(Constants.PriAccountsFile, jsonContent);
        }


        private void btn_flash_click(object sender, RoutedEventArgs e) {
            try {
                if (!Directory.Exists(Constants.PrivateDir)) {
                    Directory.CreateDirectory(Constants.PrivateDir);
                }
            }
            catch (Exception ex) {
                LogManager.Log.Error("Failed to create private directory: {Exception}", ex);
            }
            try {
                if (!File.Exists(Constants.PriAccountsFile)) {
                    File.Create(Constants.PriAccountsFile).Close();
                }
            }
            catch (Exception ex) {
                LogManager.Log.Error("Failed to create private accounts file: {Exception}", ex);
            }
            // 加载私有账户
            try {
                string jsonContent = File.ReadAllText(Constants.PriAccountsFile);
                string Private_Account_json = File.ReadAllText(Constants.PriAccountsFile);
                List<PriAccount> priAccounts = JsonSerializer.Deserialize<List<PriAccount>>(Private_Account_json, Constants.JsonOptions) ?? [];
                foreach (var account in priAccounts) {
                    ListBoxItem item = new() {
                        Content = account.Name_pri,
                        Tag = account
                    };
                    User_choice.Items.Add(item);
                }
            }
            // 输出错误
            catch (Exception ex) {
                LogManager.Log.Error("Failed to load private accounts: {Exception}", ex);
            }
        }
    }
}
