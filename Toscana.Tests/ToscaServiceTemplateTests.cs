using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaServiceTemplateTests
    {
        [Test]
        public void Ctor_NodeTypes_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.NodeTypes.Should().NotBeNull();
            toscaSimpleProfile.NodeTypes.Should().HaveCount(0);
        }

        [Test]
        public void Ctor_CapabilityTypes_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.CapabilityTypes.Should().NotBeNull();
            toscaSimpleProfile.CapabilityTypes.Should().HaveCount(0);
        }

        [Test]
        public void Ctor_Imports_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.Imports.Should().NotBeNull();
            toscaSimpleProfile.Imports.Should().HaveCount(0);
        }

        [Test]
        public void Metadata_Initialized_To_Empty_Dictionary()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.Metadata.Should().NotBeNull();
            toscaSimpleProfile.Metadata.Should().HaveCount(0);
        }

        [Test]
        public void RelationshipTypes_Initialized_To_Empty_Dictionary()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.RelationshipTypes.Should().NotBeNull();
            toscaSimpleProfile.RelationshipTypes.Should().HaveCount(0);
        }

        [Test]
        public void TopologyTemplate_Should_Be_Initialized()
        {
            var toscaSimpleProfile = new ToscaServiceTemplate();

            toscaSimpleProfile.TopologyTemplate.Should().NotBeNull();
        }

        [Test]
        public void ToscaDefinitionsVersion_Invalid_ValidationExceptionThrown()
        {
            // Arrange
            var toscaSimpleProfile = new ToscaServiceTemplate
            {
                ToscaDefinitionsVersion = "INVALID"
            };

            var toscaValidator = new ToscaValidator<ToscaServiceTemplate>();

            // Act
            Action action = () => toscaValidator.Validate(toscaSimpleProfile);

            // Assert
            action.ShouldThrow<ToscaValidationException>()
                .WithMessage("tosca_definitions_version shall be tosca_simple_yaml_1_0");
        }

        [Test]
        public void ToscaDefinitionsVersion_Valid_NoException()
        {
            // Arrange
            var toscaSimpleProfile = new ToscaServiceTemplate
            {
                ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
            };

            var toscaValidator = new ToscaValidator<ToscaServiceTemplate>();

            // Act
            Action action = () => toscaValidator.Validate(toscaSimpleProfile);

            // Assert
            action.ShouldNotThrow<Exception>();
        }

        [Test]
        public void Parse_Tosca_Yaml_From_Stream_Succeeds()
        {
            const string toscaString = @"
tosca_definitions_version: tosca_simple_yaml_1_0
 
node_types:
  example.TransactionSubsystem:
    properties:
      num_cpus:
        type: integer
        description: Number of CPUs requested for a software node instance.
        default: 1
        status: experimental
        required: true
        entry_schema: default
        constraints:
          - valid_values: [ 1, 2, 4, 8 ]";

            var tosca = ToscaServiceTemplate.Parse(toscaString.ToMemoryStream());

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().BeNull();
            tosca.NodeTypes.Should().HaveCount(1);

            var nodeType = tosca.NodeTypes["example.TransactionSubsystem"];

            nodeType.Properties.Should().HaveCount(1);
            var numCpusProperty = nodeType.Properties["num_cpus"];
            numCpusProperty.Type.Should().Be("integer");
            numCpusProperty.Description.Should().Be("Number of CPUs requested for a software node instance.");
            numCpusProperty.Default.Should().Be("1");
            numCpusProperty.Required.Should().BeTrue();
            numCpusProperty.Status.Should().Be(ToscaPropertyStatus.experimental);
            numCpusProperty.EntrySchema.Should().Be("default");
            numCpusProperty.Constraints.Should().HaveCount(1);
            numCpusProperty.Constraints.Single().Should().HaveCount(1);
            var validValues = (List<object>)numCpusProperty.Constraints.Single()["valid_values"];
            validValues.Should().BeEquivalentTo(new List<object> { "1", "2", "4", "8" });
        }
    }
}