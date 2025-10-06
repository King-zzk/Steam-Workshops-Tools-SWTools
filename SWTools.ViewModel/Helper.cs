namespace SWTools.ViewModel {
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static class Helper {
        // 字节数转字符串
        public static string ByteToString(long bytes) {
            float fbytes = bytes;
            if (bytes < 800) return bytes.ToString() + " B";
            fbytes /= 1024;
            if (fbytes < 800) return fbytes.ToString("#.##") + " KiB";
            fbytes /= 1024;
            if (fbytes < 800) return fbytes.ToString("#.##") + " MiB";
            fbytes /= 1024;
            return fbytes.ToString("#.##") + " GiB";
        }
    }
}
