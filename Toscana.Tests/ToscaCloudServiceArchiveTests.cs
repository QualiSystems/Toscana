using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaCloudServiceArchiveTests
    {
        [Test]
        public void ToscaMetadata_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive();

            // Assert
            toscaCloudServiceArchive.ToscaMetadata.Should().NotBeNull();
        }

        [Test]
        public void ToscaServiceTemplates_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive();

            // Assert
            toscaCloudServiceArchive.ToscaServiceTemplates.Should().NotBeNull();
        }

        [Test]
        public void EntryPointServiceTemplate_Returns_EntryDefinitions_Template()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive
            {
                ToscaMetadata = {EntryDefinitions = "tosca1.yaml"}
            };
            toscaCloudServiceArchive.AddToscaServiceTemplate("tosca1.yaml", new ToscaServiceTemplate {Description = "tosca1 description"});
            toscaCloudServiceArchive.AddToscaServiceTemplate("base.yaml", new ToscaServiceTemplate {Description = "base description"});

            // Assert
            toscaCloudServiceArchive.EntryPointServiceTemplate.Description.Should().Be("tosca1 description");
        }
    }
}