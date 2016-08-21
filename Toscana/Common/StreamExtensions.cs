using System;
using System.IO;

namespace Toscana.Common
{
    /// <summary>
    ///     Stream extensions
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        ///     Reads a stream into byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (!stream.CanRead)
            {
                return new byte[0];
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (var binaryReader = new BinaryReader(stream))
            {
                const int bufferSize = 4096;
                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[bufferSize];
                    int count;
                    while ((count = binaryReader.Read(buffer, 0, buffer.Length)) != 0)
                        ms.Write(buffer, 0, count);
                    return ms.ToArray();
                }
            }
        }
    }
}