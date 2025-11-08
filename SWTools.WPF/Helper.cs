using System.IO;
using System.Reflection;
using System.Text;

namespace SWTools.WPF {
    internal class Helper {
        // 将 Markdown 转换为可直接展示的 HTML
        public static string MdToHtml(string markdown) {
            var assembly = Assembly.GetExecutingAssembly();
            var css = GetEmbeddedResource("SWTools.WPF.Resources.markdown.css");
            var content = CommonMark.CommonMarkConverter.Convert(markdown);
            return $@"
<html>
<head><meta charset=""UTF-8""><style>{css}</style></head>
<body>{content}</body>
</html>
";
        }

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
