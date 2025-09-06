using System;
using System.Text.Json;

namespace SWTools.Core {
    internal class Program {
        private static void Main(string[] args) {
            // 启动日志器
            Helper.SetupLogger();

            //Console.WriteLine($"Lastest version: {Helper.RequestLatestVersion()}");

            Item item1 = new("3492532274");
            Item item2 = new("3543159422");
            ItemList items = [item1, item2];
            items.ParseAll();
            items.Save("items.json");
        }
    }
}
