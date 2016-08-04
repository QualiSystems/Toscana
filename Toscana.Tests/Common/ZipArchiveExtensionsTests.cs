using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Tests.Engine;

namespace Toscana.Tests.Common
{
    [TestFixture]
    public class ZipArchiveExtensionsTests
    {
        [Test]
        public void ArchiveEntryDictionary_Should_Be_Agnostic_To_Directory_Delimiter()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.CreateArchive("tosca.zip", new[] {new FileContent(@"location/file.yaml", "YAML")});
            var archiveEntriesDictionary =
                new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open)).GetArchiveEntriesDictionary();

            Action action = () =>
            {
                var zipArchiveEntry = archiveEntriesDictionary[@"location\file.yaml"];
                zipArchiveEntry.Should().NotBeNull();
            };

            action.ShouldNotThrow();
        }        
        
        [Test]
        public void Directories_Should_Not_Return()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.CreateArchive("tosca.zip", new[]
            {
                new FileContent(@"location/file.yaml", "some yaml content"),
                new FileContent(@"spec/", ""),
                new FileContent(@"icons/", "")
            });
            var archiveEntriesDictionary =
                new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open)).GetArchiveEntriesDictionary();

            // Assert
            var zipArchiveEntries = archiveEntriesDictionary.ToDictionary(a=>a.Key, b=>b.Value);
            zipArchiveEntries.Should().ContainKey("location/file.yaml");
            zipArchiveEntries.Should().HaveCount(1);
        }
    }
}