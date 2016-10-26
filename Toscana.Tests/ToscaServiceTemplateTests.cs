using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            var tosca = ToscaServiceTemplate.Load(toscaString.ToMemoryStream());

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
            numCpusProperty.EntrySchema.Type.Should().Be("default");
            numCpusProperty.Constraints.Should().HaveCount(1);
            numCpusProperty.Constraints.Single().Should().HaveCount(1);
            var validValues = (List<object>)numCpusProperty.Constraints.Single()["valid_values"];
            validValues.Should().BeEquivalentTo(new List<object> { "1", "2", "4", "8" });
        }

        [Test]
        public void Load_Service_Template_From_Stream_And_Save_Succeeds()
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

            var serviceTemplate = ToscaServiceTemplate.Load(toscaString.ToMemoryStream());
            byte[] savedTemplateBuffer;
            using (var memoryStream = new MemoryStream())
            {
                serviceTemplate.Save(memoryStream);
                memoryStream.Flush();

                savedTemplateBuffer = memoryStream.GetBuffer();
            }

            var loadedAfterSaveTemplate = ToscaServiceTemplate.Load(new MemoryStream(savedTemplateBuffer));

            // Assert

            loadedAfterSaveTemplate.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            loadedAfterSaveTemplate.Description.Should().BeNull();
            loadedAfterSaveTemplate.NodeTypes.Should().HaveCount(1);

            var nodeType = serviceTemplate.NodeTypes["example.TransactionSubsystem"];

            nodeType.Properties.Should().HaveCount(1);
            var numCpusProperty = nodeType.Properties["num_cpus"];
            numCpusProperty.Type.Should().Be("integer");
            numCpusProperty.Description.Should().Be("Number of CPUs requested for a software node instance.");
            numCpusProperty.Default.Should().Be("1");
            numCpusProperty.Required.Should().BeTrue();
            numCpusProperty.Status.Should().Be(ToscaPropertyStatus.experimental);
            numCpusProperty.EntrySchema.Type.Should().Be("default");
            numCpusProperty.Constraints.Should().HaveCount(1);
            numCpusProperty.Constraints.Single().Should().HaveCount(1);
            var validValues = (List<object>)numCpusProperty.Constraints.Single()["valid_values"];
            validValues.Should().BeEquivalentTo(new List<object> { "1", "2", "4", "8" });
        }

        [Test]
        public void Property_Default_Are_Loaded_When_Not_Specified()
        {
            const string toscaString = @"
tosca_definitions_version: tosca_simple_yaml_1_0
 
node_types:
  example.TransactionSubsystem:
    properties:
      num_cpus:
        type: integer
";

            var serviceTemplate = ToscaServiceTemplate.Load(toscaString.ToMemoryStream());

            // Assert
            var numCpusProperty = serviceTemplate.NodeTypes["example.TransactionSubsystem"].Properties["num_cpus"];
            numCpusProperty.Type.Should().Be("integer");
            numCpusProperty.Description.Should().BeNull();
            numCpusProperty.Default.Should().BeNull();
            numCpusProperty.Required.Should().BeTrue();
            numCpusProperty.Status.Should().Be(ToscaPropertyStatus.supported);
            numCpusProperty.EntrySchema.Should().BeNull();
            numCpusProperty.Constraints.Should().BeEmpty();
        }

        [Test]
        public void Service_Template_With_Complex_Data_Type_Can_Be_Parsed()
        {
            string toscaServiceTemplate = @"
tosca_definitions_version: tosca_simple_yaml_1_0

node_types:
    tosca.nodes.SoftwareComponent:
        derived_from: tosca.nodes.Root
        properties:
            # domain-specific software component version
            component_version:
                type: version
                required: false
            admin_credential:
                type: tosca.datatypes.Credential
                required: false
        requirements:
        - host:
            capability: tosca.capabilities.Container
            node: tosca.nodes.Compute
            relationship: tosca.relationships.HostedOn";

            var serviceTemplate = ToscaServiceTemplate.Load(toscaServiceTemplate.ToMemoryStream());

            var toscaMetadata = new ToscaMetadata
                { CsarVersion = new Version(1,1), EntryDefinitions = "tosca.yml", ToscaMetaFileVersion = new Version(1,1), CreatedBy = "anonymous" };
            var cloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yml", serviceTemplate);

            List<ValidationResult> results;
            cloudServiceArchive.TryValidate(out results)
                .Should()
                .BeTrue(string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage)));
        }

        [Test]
        [Ignore]
        public void Exception_Thrown_When_Cyclic_Dependency_Between_Node_Types_Exists()
        {
            string toscaServiceTemplate = @"
tosca_definitions_version: tosca_simple_yaml_1_0

node_types:
    tosca.nodes.SoftwareComponent:
        derived_from: tosca.nodes.BasicComponent
        properties:
            # domain-specific software component version
            component_version:
                type: version
                required: false
            admin_credential:
                type: tosca.datatypes.Credential
                required: false
    tosca.nodes.BasicComponent:
        derived_from: tosca.nodes.SoftwareComponent
";

            var serviceTemplate = ToscaServiceTemplate.Load(toscaServiceTemplate.ToMemoryStream());

            var toscaMetadata = new ToscaMetadata
                { CsarVersion = new Version(1,1), EntryDefinitions = "tosca.yml", ToscaMetaFileVersion = new Version(1,1), CreatedBy = "anonymous" };
            var cloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yml", serviceTemplate);

            List<ValidationResult> results;
            cloudServiceArchive.TryValidate(out results).Should().BeFalse();
            results.Should().ContainSingle(r => r.ErrorMessage.Equals("Circular dependency detected on NodeType: 'tosca.nodes.BasicComponent'"));
            results.Should().ContainSingle(r => r.ErrorMessage.Equals("Circular dependency detected on NodeType: 'tosca.nodes.SoftwareComponent'"));
        }

        [Test]
        public void Exception_Thrown_When_Cyclic_Dependency_Between_Capability_Types_Exists()
        {
            string toscaServiceTemplate = @"
tosca_definitions_version: tosca_simple_yaml_1_0

capability_types:
    tosca.types.SoftwareComponent:
        derived_from: tosca.types.BasicComponent
    tosca.types.BasicComponent:
        derived_from: tosca.types.SoftwareComponent
";

            var serviceTemplate = ToscaServiceTemplate.Load(toscaServiceTemplate.ToMemoryStream());

            var toscaMetadata = new ToscaMetadata
                { CsarVersion = new Version(1,1), EntryDefinitions = "tosca.yml", ToscaMetaFileVersion = new Version(1,1), CreatedBy = "anonymous" };
            var cloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yml", serviceTemplate);

            List<ValidationResult> results;
            cloudServiceArchive.TryValidate(out results).Should().BeFalse();
            results.Should().ContainSingle(r => r.ErrorMessage.Equals("Circular dependency detected on CapabilityType: 'tosca.types.BasicComponent'"));
            results.Should().ContainSingle(r => r.ErrorMessage.Equals("Circular dependency detected on CapabilityType: 'tosca.types.SoftwareComponent'"));
        }
    }
}