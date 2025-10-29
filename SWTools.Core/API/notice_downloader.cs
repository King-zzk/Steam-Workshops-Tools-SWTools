using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;


namespace SWTools.Core.API {
    public class notice_downloader {
        // 下载公告
        static string download_notice_url = "https://raw.githubusercontent.com/King-zzk/Steam-Workshops-Tools-SWTools/refs/heads/master/api/notice";
        static string save_file = "./data/notice";

        public static async Task<bool> Fetch() {
            LogManager.Log.Information("Fetching notice");
            var response = await Helper.Http.MakeGithubGet(download_notice_url);
            if (response == null) return false;
            try {
                using (StreamWriter sw = new(save_file)) {
                    sw.Write(response.ToString());
                }
                return true;
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when saving {FileName}:\n{Exception}",
                    save_file, ex);
                return false;
            }
        }
    }
}
