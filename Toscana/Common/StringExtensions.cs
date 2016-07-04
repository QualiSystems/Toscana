using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Toscana.Common
{
    public static class StringExtensions
    {
        public static bool EqualsAny(this string str, params string[] args)
        {
            return args.Any(x =>
              StringComparer.InvariantCultureIgnoreCase.Equals((string) x, str));
        }

        public static Stream ToMemoryStream(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Returns byte array of given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">Encoding to use for the conversion. Optional. Default is Unicode</param>
        /// <returns>Byte array representing the string</returns>
        public static byte[] ToByteArray(this string str, Encoding encoding = null)
        {
            return (encoding ?? Encoding.Unicode).GetBytes(str);
        }
    }
}