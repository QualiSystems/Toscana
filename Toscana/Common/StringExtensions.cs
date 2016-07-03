using System;
using System.IO;
using System.Linq;

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
    }
}