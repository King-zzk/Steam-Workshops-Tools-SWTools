using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;


namespace SWTools.Core.API {
        public class notice_downloader {
        // 下载公告
        string download_notice_url = "https://raw.githubusercontent.com/King-zzk/Steam-Workshops-Tools-SWTools/refs/heads/master/api/notice";
        string save_file = "./data/notice";
        public async void DownloadNotice() {
            try {
                string? response = await Helper.Http.MakeGithubGet(download_notice_url);
                if (response != null) {
                    using (StreamWriter sw = new(save_file)) {
                        sw.Write(response);
                    }
                }
            }
            catch (Exception ex) {
                LogManager.Log.Error("Exception occured when downloading notice:\n{Exception}", ex);
            }
        }
    }
}
