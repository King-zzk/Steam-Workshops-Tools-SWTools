using System;
using System.Net.Mime;
using System.Text.Json;

namespace SWTools.Core {
    internal class Program {
        private static void Main(string[] args) {
            // 启动
            Helper.Main.SetupAll();

            //using (var task = API.LatestInfo.Request()) {
            //    task.Wait();
            //    Console.WriteLine(task.Result);
            //}

            //using (var task = API.PubAccounts.Request()) {
            //    task.Wait();
            //    Console.WriteLine(task.Result);
            //}

            // 结束
            Helper.Main.CleanupAll();
        }
    }
}
