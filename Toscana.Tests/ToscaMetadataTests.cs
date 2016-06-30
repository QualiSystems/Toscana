using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaMetadataTests
    {
        private IToscaParser<ToscaMetadata> metadataParser;

        [SetUp]
        public void SetUp()
        {
            metadataParser = new Bootstrapper().GetToscaMetadataParser();
        }

        [Test]
        public void ToscaMetadata_Deserialized_Successfully()
        {
            var toscaMetadata = metadataParser.Parse(
@"TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions/tosca_elk.yaml");

            toscaMetadata.ToscaMetaFileVersion.Should().Be(new Version("1.0"));
            toscaMetadata.CsarVersion.Should().Be(new Version("1.1"));
            toscaMetadata.CreatedBy.Should().Be("OASIS TOSCA TC");
            toscaMetadata.EntryDefinitions.Should().Be("definitions/tosca_elk.yaml");
        }

        [Test]
        public void ValidationException_Thrown_When_Definition_Empty()
        {
            Action action = () => metadataParser.Parse(
@"TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC");

            action.ShouldThrow<ToscaValidationException>().WithMessage("Entry-Definitions is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_CreatedBy_Empty()
        {
            Action action = () => metadataParser.Parse(
@"TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Entry-Definitions: definitions/tosca_elk.yaml");

            action.ShouldThrow<ToscaValidationException>().WithMessage("Created-By is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_CSARVersion_Empty()
        {
            Action action = () => metadataParser.Parse(
@"TOSCA-Meta-File-Version: 1.0
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions/tosca_elk.yaml");

            action.ShouldThrow<ToscaValidationException>().WithMessage("CSAR-Version is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_TOSCAMetaFileVersion_Empty()
        {
            Action action = () => metadataParser.Parse(
@"Created-By: OASIS TOSCA TC
CSAR-Version: 1.1
Entry-Definitions: definitions/tosca_elk.yaml");

            action.ShouldThrow<ToscaValidationException>().WithMessage("TOSCA-Meta-File-Version is required in TOSCA.meta");
        }
    }
}