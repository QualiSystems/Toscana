using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Toscana.Engine;

namespace Toscana.Common
{
    /// <summary>
    /// Extensions for ZipArhive
    /// </summary>
    internal static class ZipArchiveExtensions
    {
        /// <summary>
        /// Gets a dictionary of ZipArchiveEntry
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, ZipArchiveEntry> GetArchiveEntriesDictionary(
            this ZipArchive archive)
        {
            var zipArchiveEntries = new Dictionary<string, ZipArchiveEntry>(new PathEqualityComparer());
            foreach (var zipArchiveEntry in archive.Entries)
            {
                zipArchiveEntries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
            }
            return zipArchiveEntries;
        }
    }
}