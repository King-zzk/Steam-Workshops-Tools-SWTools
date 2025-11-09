namespace SWTools.ViewModel {
    /// <summary>
    /// 辅助方法
    /// </summary>
    public static class Helper {
        // 字节数转字符串
        public static string ByteToString(long bytes) {
            float fbytes = bytes;
            if (bytes < 1000) return bytes.ToString() + " B";
            fbytes /= 1024;
            if (fbytes < 1000) return fbytes.ToString("0.##") + " KiB";
            fbytes /= 1024;
            if (fbytes < 1000) return fbytes.ToString("0.##") + " MiB";
            fbytes /= 1024;
            return fbytes.ToString("0.##") + " GiB";
        }
    }
}
