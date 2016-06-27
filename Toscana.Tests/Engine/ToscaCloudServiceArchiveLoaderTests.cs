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
        private IFileSystem fileSystem;
        private IToscaCloudServiceArchiveLoader toscaCloudServiceArchiveLoader;

        [SetUp]
        public void SetUp()
        {
            fileSystem = new MockFileSystem();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Replace(fileSystem);
            toscaCloudServiceArchiveLoader = bootstrapper.GetToscaCloudServiceArchiveLoader();
        }

        [Test]
        public void FileNotFoundException_Should_Be_Thrown_When_Archive_Does_Not_Exist()
        {
            // Act
            Action action = () => toscaCloudServiceArchiveLoader.Load("non_existing.zip");

            // Assert
            action.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void EntryPointNotFoundException_Should_Be_Thrown_When_EntryPoint_File_Does_Not_Exist()
        {
            // Arrange
            var fileContents = new List<FileContent> {new FileContent("TOSCA.meta", "Entry-Definitions: not_existing.yaml")};
            CreateArchive(fileSystem, "tosca.zip", fileContents);

            // Act
            Action action = () => toscaCloudServiceArchiveLoader.Load("tosca.zip");

            // Assert
            action.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Exception_Should_Be_Thrown_When_Tosca_Meta_File_Does_Not_Exist()
        {
            // Arrange
            CreateArchive(fileSystem, "tosca.zip", new FileContent[0]);

            // Act
            Action action = () => toscaCloudServiceArchiveLoader.Load("tosca.zip");

            // Assert
            action.ShouldThrow<FileNotFoundException>().WithMessage("TOSCA.meta file not found within TOSCA Cloud Service Archive file.");
        }

        [Test] 
        public void Tosca_Cloud_Service_Archive_With_Single_Profile_Should_Be_Parsed()
        {
            // Arrange
            var toscaMetaContent = @"TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions\tosca_elk.yaml";
            var toscaSimpleProfileContent = @"
tosca_definitions_version: tosca_simple_yaml_1_0
 
node_types:
  example.TransactionSubsystem:
    properties:
      num_cpus:
        type: integer";

            var fileContents = new List<FileContent>
            {
                new FileContent("TOSCA.meta", toscaMetaContent),
                new FileContent(@"definitions\tosca_elk.yaml", toscaSimpleProfileContent)
            };

            CreateArchive(fileSystem, "tosca.zip", fileContents);

            // Act
            var toscaCloudServiceArchive = toscaCloudServiceArchiveLoader.Load("tosca.zip");

            // Assert
            toscaCloudServiceArchive.ToscaSimpleProfiles.Should().HaveCount(1);
            toscaCloudServiceArchive.ToscaSimpleProfiles[@"definitions\tosca_elk.yaml"].NodeTypes["example.TransactionSubsystem"].Properties["num_cpus"].Type.Should().Be("integer");
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