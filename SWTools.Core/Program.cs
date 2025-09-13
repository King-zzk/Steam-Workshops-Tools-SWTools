using System;
using System.Net.Mime;
using System.Text.Json;

namespace SWTools.Core {
    internal class Program {
        private static void Main(string[] args) {
            // 启动
            ConfigManager.Setup();
            LogManager.Setup();

            //Helper.SetupSteamcmd().Wait();

            Item item = new("3543159422");
            item.Parse().Wait();
            Console.WriteLine(item.UrlPreview);
            Helper.DownloadImage(item.UrlPreview,"preview").Wait();
            //item.Download(AccountManager.GetAccountFor(item.AppId)[0]);
            //Console.WriteLine(item.ToString());


            // 结束
            ConfigManager.Save("Exit");
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
            items.ParseAll().Wait();
            items.Save("items.json");
        }
    }
}
