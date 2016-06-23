using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaMetadataTests
    {
        [Test]
        public void ToscaMetadata_Deserialized_Successfully()
        {
            var yamlDeserializer = new ToscaMetadataDeserializer();

            var toscaMetadata = yamlDeserializer.Deserialize(
@"TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions/tosca_elk.yaml");

            toscaMetadata.ToscaMetaFileVersion.Should().Be(new Version("1.0"));
            toscaMetadata.CsarVersion.Should().Be(new Version("1.1"));
            toscaMetadata.CreatedBy.Should().Be("OASIS TOSCA TC");
            toscaMetadata.EntryDefinitions.Should().Be("definitions/tosca_elk.yaml");
        }
    }
}