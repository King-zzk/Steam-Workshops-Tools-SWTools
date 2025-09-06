using System;
using System.Text.Json;

namespace SWTools.Core {
    internal class Program {
        private static void Main(string[] args) {
            // 启动
            ConfigManager.Load();
            Helper.SetupLogger();

            Helper.SetupSteamcmd();

            Item item = new("3492532274");
            item.Parse();
            item.Download(AccountManager.GetAccountFor(item.AppId)[0]);
            Console.WriteLine(item.ToString());


            // 结束
            ConfigManager.Save();
        }

        // 下面是一些测试用方法

        // 测试检查更新
        private static void TestUpdate() {
            Console.WriteLine($"Lastest version: {Helper.RequestLatestVersion()}");
        }
        // 测试批量解析
        private static void TestBatchParse() {
            ItemList items = [];
            items.Add(new("3492532274"));
            items.Add(new("3543159422"));
            items.ParseAll();
            items.Save("items.json");
        }
    }
}
