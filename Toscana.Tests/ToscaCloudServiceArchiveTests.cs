using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
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
        public void GetArtifactsBytes_Should_Return_Artifact_Content()
        {
            // Act
            var fileSystem = new MockFileSystem();
            fileSystem.CreateArchive("tosca.zip", new []{new FileContent("device.png", "IMAGE_CONTENT")});
            var zipArchive = new ZipArchive(fileSystem.File.Open("tosca.zip", FileMode.Open));
            var zipArchiveEntries = zipArchive.Entries.ToDictionary(e => e.FullName, e => e);

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
    }
}