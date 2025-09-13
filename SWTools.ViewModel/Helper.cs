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
            if (bytes < 100) return bytes.ToString() + "B";
            bytes /= 1024;
            if (bytes < 100) return bytes.ToString("#.##") + "KiB";
            bytes /= 1024;
            if (bytes < 100) return bytes.ToString("#.##") + "MiB";
            bytes /= 1024;
            return bytes.ToString("#.##") + "GiB";
        }
    }
}
