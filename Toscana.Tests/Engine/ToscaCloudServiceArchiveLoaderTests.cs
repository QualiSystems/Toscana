using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaCloudServiceArchiveLoaderTests
    {
        [Test]
        public void FileNotFoundException_Should_Be_Thrown_When_Archive_Does_Not_Exist()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            var toscaCloudServiceArchiveLoader = new ToscaCloudServiceArchiveLoader(fileSystem);

            // Act
            Action action = () => toscaCloudServiceArchiveLoader.Load("non_existing.zip");

            // Assert
            action.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void EntryPointNotFoundException_Should_Be_Thrown_When_EntryPoint_File_Does_Not_Exist()
        {
            // Arrange
            const string toscaMetaContent = @"
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: tosca_elk.yaml";

            var fileSystem = new MockFileSystem();
            var fileContents = new List<FileContent> {new FileContent("TOSCA.meta", toscaMetaContent)};
            CreateArchive(fileSystem, "tosca.zip", fileContents);

            var toscaCloudServiceArchiveLoader = new ToscaCloudServiceArchiveLoader(fileSystem);

            // Act
            Action action = () => toscaCloudServiceArchiveLoader.Load("tosca.zip");

            // Assert
            action.ShouldThrow<FileNotFoundException>();
        }

        private static void CreateArchive(IFileSystem fileSystem, string archiveFilePath, IEnumerable<FileContent> fileContents)
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