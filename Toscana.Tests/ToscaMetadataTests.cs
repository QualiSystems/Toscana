using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
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
            metadataParser = Bootstrapper.GetToscaMetadataParser();
        }

        [Test]
        public void ToscaMetadata_Deserialized_Successfully()
        {
            var toscaMeta = @"
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions/tosca_elk.yaml";

            using (var memoryStream = toscaMeta.ToMemoryStream())
            {
                var toscaMetadata = metadataParser.Parse(memoryStream);

                toscaMetadata.ToscaMetaFileVersion.Should().Be(new Version("1.0"));
                toscaMetadata.CsarVersion.Should().Be(new Version("1.1"));
                toscaMetadata.CreatedBy.Should().Be("OASIS TOSCA TC");
                toscaMetadata.EntryDefinitions.Should().Be("definitions/tosca_elk.yaml");
            }
        }

        [Test]
        public void ValidationException_Thrown_When_Definition_Empty()
        {
            var toscaMeta = @"
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC";

            Action action = () =>
            {
                using (var memoryStream = toscaMeta.ToMemoryStream())
                {
                    metadataParser.Parse(memoryStream);
                }
            };

            action.ShouldThrow<ToscaValidationException>().WithMessage("Entry-Definitions is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_CreatedBy_Empty()
        {
            var toscaMeta = @"
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Entry-Definitions: definitions/tosca_elk.yaml";

            Action action = () =>
            {
                using (var memoryStream = toscaMeta.ToMemoryStream())
                {
                    metadataParser.Parse(memoryStream);
                }
            };

            action.ShouldThrow<ToscaValidationException>().WithMessage("Created-By is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_CSARVersion_Empty()
        {
            var toscaMeta = @"
TOSCA-Meta-File-Version: 1.0
Created-By: OASIS TOSCA TC
Entry-Definitions: definitions/tosca_elk.yaml";

            Action action = () =>
            {
                using (var memoryStream = toscaMeta.ToMemoryStream())
                {
                    metadataParser.Parse(memoryStream);
                }
            };

            action.ShouldThrow<ToscaValidationException>().WithMessage("CSAR-Version is required in TOSCA.meta");
        }

        [Test]
        public void ValidationException_Thrown_When_TOSCAMetaFileVersion_Empty()
        {
            var toscaMeta = @"
Created-By: OASIS TOSCA TC
CSAR-Version: 1.1
Entry-Definitions: definitions/tosca_elk.yaml";

            Action action = () =>
            {
                using (var memoryStream = toscaMeta.ToMemoryStream())
                {
                    metadataParser.Parse(memoryStream);
                }
            };

            action.ShouldThrow<ToscaValidationException>()
                    .WithMessage("TOSCA-Meta-File-Version is required in TOSCA.meta");
        }
    }
}