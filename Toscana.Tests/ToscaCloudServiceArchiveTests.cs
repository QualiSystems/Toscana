using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Exceptions;
using Toscana.Tests.Engine;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaCloudServiceArchiveTests
    {
        [Test]
        public void EntryPointServiceTemplate_Returns_EntryDefinitions_Template()
        {
            // Act
            var toscaCloudServiceArchive =
                new ToscaCloudServiceArchive(new ToscaMetadata {EntryDefinitions = "tosca1.yaml"});
            toscaCloudServiceArchive.AddToscaServiceTemplate("tosca1.yaml",
                new ToscaServiceTemplate {Description = "tosca1 description"});
            toscaCloudServiceArchive.AddToscaServiceTemplate("base.yaml",
                new ToscaServiceTemplate {Description = "base description"});

            // Assert
            toscaCloudServiceArchive.EntryPointServiceTemplate.Description.Should().Be("tosca1 description");
        }

        [Test]
        public void GetArtifactBytes_Should_Return_Empty_Array_When_File_Is_Empty()
        {
            // Arrange
            var toscaMetadata = new ToscaMetadata
            {
                CreatedBy = "Devil",
                CsarVersion = new Version(1, 1),
                ToscaMetaFileVersion = new Version(1, 0),
                EntryDefinitions = @"definitions/tosca_elk.yaml"
            };

            var fileSystem = new MockFileSystem();
            using (var zipArchive = new ZipArchive(fileSystem.File.Create("tosca.zip"), ZipArchiveMode.Create))
            {
                zipArchive.CreateEntry("some_icon.png");
            }

            var archiveEntriesDictionary =
                new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open)).GetArchiveEntriesDictionary();

            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata, archiveEntriesDictionary);
            var toscaServiceTemplate = new ToscaServiceTemplate {Description = "Devil created the world."};
            var toscaNodeType = new ToscaNodeType();
            toscaNodeType.Artifacts.Add("icon", new ToscaArtifact {File = "some_icon.png"});
            toscaServiceTemplate.NodeTypes.Add("nut-shell", toscaNodeType);

            // Act
            toscaCloudServiceArchive.AddToscaServiceTemplate(@"definitions/tosca_elk.yaml", toscaServiceTemplate);

            // Assert
            toscaCloudServiceArchive.GetArtifactBytes("some_icon.png").Should().BeEmpty();
        }

        [Test]
        public void GetArtifactsBytes_Should_Return_Artifact_Content()
        {
            // Act
            var fileSystem = new MockFileSystem();
            fileSystem.CreateArchive("tosca.zip", new[] {new FileContent("device.png", "IMAGE_CONTENT")});
            var zipArchive = new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open));
            var zipArchiveEntries = zipArchive.GetArchiveEntriesDictionary();

            var toscaNodeType = new ToscaNodeType();
            toscaNodeType.Artifacts.Add("icon", new ToscaArtifact
            {
                File = "device.png"
            });
            var toscaServiceTemplate = new ToscaServiceTemplate();
            toscaServiceTemplate.NodeTypes.Add("device", toscaNodeType);
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                EntryDefinitions = "tosca.yaml"
            }, zipArchiveEntries);

            // Act
            toscaCloudServiceArchive.AddToscaServiceTemplate("definition", toscaServiceTemplate);
            var artifactsBytes = toscaCloudServiceArchive.GetArtifactBytes("device.png");

            // Assert
            artifactsBytes.ShouldAllBeEquivalentTo("IMAGE_CONTENT".ToByteArray(Encoding.ASCII));
        }

        [Test]
        public void AddToscaServiceTemplate_Should_Throw_ArtifactNotFoundException_When_File_Missing_In_Archive()
        {
            // Act
            var fileSystem = new MockFileSystem();
            fileSystem.CreateArchive("tosca.zip", new FileContent[0]);
            var zipArchive = new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open));
            var zipArchiveEntries = zipArchive.GetArchiveEntriesDictionary();

            var toscaNodeType = new ToscaNodeType();
            toscaNodeType.Artifacts.Add("icon", new ToscaArtifact
            {
                File = "device.png"
            });
            var toscaServiceTemplate = new ToscaServiceTemplate();
            toscaServiceTemplate.NodeTypes.Add("device", toscaNodeType);
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                EntryDefinitions = "tosca.yaml"
            }, zipArchiveEntries);

            // Act
            Action action = () => toscaCloudServiceArchive.AddToscaServiceTemplate("definition", toscaServiceTemplate);

            // Assert
            action.ShouldThrow<ArtifactNotFoundException>()
                .WithMessage("Artifact 'device.png' not found in Cloud Service Archive.");
        }

        [Test]
        public void GetArtifactBytes_Should_Throw_ArtifactNotFoundException_When_File_Missing_In_Archive()
        {
            // Act
            var toscaServiceTemplate = new ToscaServiceTemplate();
            var toscaNodeType = new ToscaNodeType();
            toscaServiceTemplate.NodeTypes.Add("device", toscaNodeType);
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                EntryDefinitions = "tosca.yaml"
            }, new Dictionary<string, ZipArchiveEntry>());

            // Act
            Action action = () => toscaCloudServiceArchive.GetArtifactBytes("NOT_EXISTING.png");

            // Assert
            action.ShouldThrow<ArtifactNotFoundException>()
                .WithMessage("Artifact 'NOT_EXISTING.png' not found in Cloud Service Archive.");
        }

        [Test]
        public void NodeTypes_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            // Assert
            toscaCloudServiceArchive.NodeTypes.Should().NotBeNull();
        }

        [Test]
        public void ToscaMetadata_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "me",
                CsarVersion = new Version(1, 12),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(2, 23)
            });

            // Assert
            toscaCloudServiceArchive.ToscaMetadata.CreatedBy.Should().Be("me");
            toscaCloudServiceArchive.ToscaMetadata.CsarVersion.Should().Be(new Version(1, 12));
            toscaCloudServiceArchive.ToscaMetadata.EntryDefinitions.Should().Be("tosca.yaml");
            toscaCloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion.Should().Be(new Version(2, 23));
        }

        [Test]
        public void ToscaServiceTemplates_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            // Assert
            toscaCloudServiceArchive.ToscaServiceTemplates.Should().NotBeNull();
        }

        [Test]
        public void GetArtifactBytes_Should_Return_File_Content_Of_File_That_Is_Not_An_Artifact()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            var toscaMetadata = new ToscaMetadata
            {
                CreatedBy = "Devil",
                CsarVersion = new Version(1, 1),
                ToscaMetaFileVersion = new Version(1, 0),
                EntryDefinitions = @"definitions/tosca_elk.yaml"
            };

            fileSystem.CreateArchive("tosca.zip", new[]
            {
                new FileContent("nut_shell.png", "IMAGE_CONTENT")
            });

            var archiveEntriesDictionary = new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open)).GetArchiveEntriesDictionary();

            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata, archiveEntriesDictionary);
            var toscaServiceTemplate = new ToscaServiceTemplate();
            toscaServiceTemplate.NodeTypes.Add("nut-shell", new ToscaNodeType());
            toscaCloudServiceArchive.AddToscaServiceTemplate(@"definitions/tosca_elk.yaml", toscaServiceTemplate);

            // Act
            var artifactBytes = toscaCloudServiceArchive.GetArtifactBytes("nut_shell.png");

            artifactBytes.ShouldAllBeEquivalentTo("IMAGE_CONTENT".ToByteArray(Encoding.ASCII));
        }

        [Test]
        public void GetArtifactBytes_OnCloudServiceArchive_Without_Artifacts_Should_Throw_ArtifactNotFoundException()
        {
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            Action action = () => toscaCloudServiceArchive.GetArtifactBytes("not_existing_file.png");

            action.ShouldThrow<ArtifactNotFoundException>()
                .WithMessage("Artifact 'not_existing_file.png' not found in Cloud Service Archive.");
        }
    }
}