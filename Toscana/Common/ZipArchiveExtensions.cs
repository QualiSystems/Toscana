using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace Toscana.Common
{
    /// <summary>
    /// Extensions for ZipArhive
    /// </summary>
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Gets a dictionary of ZipArchiveEntry
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, ZipArchiveEntry> GetArchiveEntriesDictionary(
            this ZipArchive archive)
        {
            return archive.Entries.ToDictionary(e => e.FullName, e => e);
        }
    }
}