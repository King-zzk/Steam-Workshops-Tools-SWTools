using System;
using System.Windows;
using System.Windows.Controls;

namespace SWTools.WPF {
    /// <summary>
    /// AddTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddTaskWindow : Window {
        // ViewModel 访问点
        public ViewModel.AddTaskWindow ViewModel {
            get => (ViewModel.AddTaskWindow)DataContext;
            set { DataContext = value; }
        }

        private bool _closeFromBtnOk = false;

        public AddTaskWindow() {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            if (ViewModel.HasParsing()) {
                MsgBox msgBox = new("请等待当前解析完成", "请等待列表中所有物品完成解析再关闭此窗口。", false) { Owner = this };
                msgBox.ShowDialog();
            } else if (ViewModel.HasFailed()) {
                MsgBox msgBox = new("有物品解析失败", "有一个或多个物品信息解析失败。确认关闭此窗口吗？\n\n" +
                    "单击“否”，您可以手动补充信息，然后添加到下载列表；\n" +
                    "单击“是”，解析失败的物品不会被添加到下载列表。", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res == true) {
                    if (Owner is MainWindow owner) {
                        foreach (var item in ViewModel.Items) {
                            if (item.ParseState == Core.Item.EParseState.Failed) continue;
                            owner.ViewModel.Items.Add(item);
                        }
                    }
                    Close();
                }
            } else {
                if (Owner is not MainWindow owner) return;
                // 检查是否有重合的项
                List<string> dupItems = [];
                foreach (var item in ViewModel.Items) {
                    if (owner.ViewModel.Items.Contains(item.ItemId)) {
                        dupItems.Add(item.ItemId);
                    }
                }
                if (dupItems.Count > 0) {
                    MsgBox msgBox = new("有重复的物品", "待添加的物品中有一个或多个已在下载列表中，您是否想覆盖？\n" +
                    "单击 “否”，重复的物品不会被添加到下载列表；\n" +
                    "单击 “是”，将会覆盖下载列表中重复的物品。\n\n" +
                    "被覆盖的物品可能会被重新下载。", true) { Owner = this };
                    bool? res = msgBox.ShowDialog();
                    if (res == true) {
                        foreach (var item in dupItems) {
                            owner.ViewModel.Items.Remove(owner.ViewModel.Items.Find(item)!);
                        }
                    }
                }
                // 添加
                foreach (var item in ViewModel.Items) {
                    owner.ViewModel.Items.Add(item);
                }
                _closeFromBtnOk = true;
                Close();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (ViewModel.CheckString(TextBox.Text)) {
                ViewModel.IsBtnParseEnable = true;
            } else {
                ViewModel.IsBtnParseEnable = false;
            }
        }

        private void BtnHelpItem_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("什么是物品 ID？",
                "每个创意工坊物品有唯一的物品 ID。\n" +
                "例如，您要下载的创意工坊物品的网页位于：\n\n" +
                "https://steamcommunity.com/sharedfiles/filedetails/?id=XXXXXX\n" +
                "或 https://steamcommunity.com/sharedfiles/filedetails/XXXXXX\n\n" +
                "那么 “XXXXXX” 就是物品 ID。", false) { Owner = this };
            msgBox.ShowDialog();
        }

        private void BtnParse_Click(object sender, RoutedEventArgs e) {
            if (ViewModel.AddToQueue()) {
                ViewModel.StartParse();
            } else {
                MsgBox msgBox = new("未添加任何物品",
               "您未添加任何物品到解析列表。\n" +
               "这可能是由于解析列表中有相同的物品。", false) { Owner = this };
                msgBox.ShowDialog();
            }
        }

        private void BtnHelpApp_Click(object sender, RoutedEventArgs e) {
            MsgBox msgBox = new("什么是 App ID？",
                "每个 Steam App（包括游戏）有唯一的 ID。\n" +
                "例如，对于您要下载的物品，其所属 App 的网页位于：\n\n" +
                "https://steamcommunity.com/app/XXXXXX\n\n" +
                "那么 “XXXXXX” 就是 App ID。", false) { Owner = this };
            msgBox.ShowDialog();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ListView.SelectedItems.Count == 0) return;
            if (ListView.SelectedItems[0] is not ViewModel.DisplayItem selection) return;
            if (selection.Item.ParseState == Core.Item.EParseState.Failed ||
                selection.Item.ParseState == Core.Item.EParseState.Manual) {
                ViewModel.IsBtnEditEnable = true;
            } else {
                ViewModel.IsBtnEditEnable = false;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e) {
            if (ListView.SelectedItems.Count == 0) { // 几乎不会执行这个分支
                MsgBox msgBox = new("未选择要修改的项",
                "您似乎没有选择要修改的物品。", false) { Owner = this };
                msgBox.ShowDialog();
                return;
            }
            if (ListView.SelectedItems[0] is not ViewModel.DisplayItem selection) return;
            if (!ViewModel.CheckString(TextBoxAppId.Text)) {
                MsgBox msgBox = new("输入格式不正确",
                "App ID 只能包含数字且不能为空。", false) { Owner = this };
                msgBox.ShowDialog();
                TextBoxAppId.Text = string.Empty;
            } else {
                try {
                    ViewModel.Items[ViewModel.Items.FindIndex(selection.Item.ItemId)].AppId = long.Parse(TextBoxAppId.Text);
                    ViewModel.Items[ViewModel.Items.FindIndex(selection.Item.ItemId)].AppName = "";
                    ViewModel.Items[ViewModel.Items.FindIndex(selection.Item.ItemId)].ParseState = Core.Item.EParseState.Manual;
                }
                catch (Exception) {
                    MsgBox msgBox = new("输入格式不正确", "输入无效。", false) { Owner = this };
                    msgBox.ShowDialog();
                    TextBoxAppId.Text = string.Empty;
                }
            }
        }

        private void MenuRemove_Click(object sender, RoutedEventArgs e) {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Parent is not ContextMenu contextMenu) return;
            if (contextMenu.PlacementTarget is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ViewModel.DisplayItem item) return;
            ViewModel.Items.Remove(item.Item);
        }

        private void BtnParseRetry_Click(object sender, RoutedEventArgs e) {
            ViewModel.RetryParse();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (_closeFromBtnOk) return;
            if (ViewModel.DisplayItems.Count == 0) {
                MsgBox msgBox = new("确认关闭？", "看起来你没有添加任何物品，\n确认关闭此窗口吗？", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res != true) {
                    e.Cancel = true;
                }
            } else {
                MsgBox msgBox = new("确认关闭？", "您似乎有待添加的物品。\n请单击 “确认” 把列表中的物品添加到下载列表。\n\n单击 “是”，此窗口会关闭但不添加物品。", true) { Owner = this };
                bool? res = msgBox.ShowDialog();
                if (res != true) {
                    e.Cancel = true;
                }
            }
        }
    }
}