using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;

namespace Toscana.Tests
{
    public static class FileSystemExtentions
    {
        public static void CreateArchive(this IFileSystem fileSystem, string archiveFilePath, IEnumerable<FileContent> fileContents)
        {
            using (var stream = fileSystem.File.Create(archiveFilePath))
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    foreach (var fileContent in fileContents)
                    {
                        var zipArchiveEntry = zipArchive.CreateEntry(fileContent.Filename);
                        using (var writer = new StreamWriter(zipArchiveEntry.Open()))
                        {
                            writer.Write(fileContent.Content);
                        }
                    }
                }
            }
        }
    }
}