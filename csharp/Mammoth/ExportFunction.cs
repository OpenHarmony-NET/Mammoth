using System.Runtime.InteropServices;
using System.Text.Json;

namespace Mammoth
{
    public static unsafe class ExportFunction
    {
        public static string ConvertToHtml(sbyte* path, bool with_img = false)
        {
            try
            {
                var path_str = Marshal.PtrToStringAuto((IntPtr)path);
                if (!string.IsNullOrEmpty(path_str))
                {
                    var converter = new DocumentConverter();
                    if (with_img)
                    {
                        converter.ImageConverter(image =>
                        {
                            using var stream = image.GetStream();
                            return new Dictionary<string, string> { { "src", $"data:{image.ContentType};base64,{stream.StreamToBase64()}" } };
                        });
                    }
                    var result = converter.ConvertToHtml(path_str);
                    return result.Value;
                }
            }
            catch { }
            return string.Empty;
        }
        public static string ExtractRawText(sbyte* path)
        {
            try
            {
                var path_str = Marshal.PtrToStringAuto((IntPtr)path);
                if (!string.IsNullOrEmpty(path_str))
                {
                    var converter = new DocumentConverter();
                    var result = converter.ExtractRawText(path_str);
                    return result.Value;
                }
            }
            catch { }
            return string.Empty;
        }
        private static string StreamToBase64(this Stream stream)
        {
            try
            {
                byte[] bytes = new byte[stream.Length];
                stream.ReadExactly(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                return Convert.ToBase64String(bytes);
            }
            catch { }
            return string.Empty;
        }
    }
}