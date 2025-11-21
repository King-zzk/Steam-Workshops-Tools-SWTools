using System.IO;
using System.Reflection;
using System.Text;

namespace SWTools.WPF {
    internal class Helper {
        // 获取嵌入的资源
        public static string GetEmbeddedResource(string resourceName) {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName)) {
                if (stream == null) {
                    return string.Empty;
                }
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
