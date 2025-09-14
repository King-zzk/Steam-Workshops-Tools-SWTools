using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWTools.ViewModel {
    /// <summary>
    /// 辅助方法
    /// </summary>
    internal static class Helper {
        public static string ByteToString(long bytes) {
            float fbytes = bytes;
            if (bytes < 800) return bytes.ToString() + " B";
            fbytes /= 1024;
            if (fbytes < 800) return fbytes.ToString("#.##") + " KiB";
            fbytes /= 1024;
            if (fbytes < 800) return fbytes.ToString("#.##") + " MiB";
            fbytes /= 1024;
            return bytes.ToString("#.##") + " GiB";
        }
    }
}
