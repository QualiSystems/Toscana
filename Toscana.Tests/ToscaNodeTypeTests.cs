using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaNodeTypeTests
    {
        [Test]
        public void Capabilities_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Capabilities.Should().HaveCount(0);
        }

        [Test]
        public void Artifacts_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Artifacts.Should().HaveCount(0);
        }

        [Test]
        public void Attributes_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Attributes.Should().HaveCount(0);
        }

        [Test]
        public void Interfaces_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Interfaces.Should().HaveCount(0);
        }

        [Test]
        public void Properties_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Properties.Should().HaveCount(0);
        }

        [Test]
        public void Requirements_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Requirements.Should().HaveCount(0);
        }

        [Test]
        public void AddRequirement_Requirement_Exist()
        {
            // Arrange
            var toscaNodeType = new ToscaNodeType();
            var toscaRequirement = new ToscaRequirement()
            {
                Node = "port"
            };

            // Act
            toscaNodeType.AddRequirement("device", toscaRequirement);

            // Assert
            toscaNodeType.Requirements.Single()["device"].Node.Should().Be("port");
        }

        [Test]
        public void When_Derived_From_Node_Type_Not_Found_Proper_Error_Message_Should_Be_In_Place()
        {
            // Arrange
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Act
            var validationResults = new List<ValidationResult>();
            Action action = () => cloudServiceArchive.TryValidate(out validationResults);

            // Assert
            action.ShouldThrow<ToscaNodeTypeNotFoundException>()
                .WithMessage("Node type 'cloudshell.nodes.Switch' not found");
        }

        [Test]
        public void GetAllProperties_Return_Properties_Of_Base_Node_Types()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Properties.Add("speed",new ToscaProperty { Type = "string"} );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("storage", new ToscaProperty { Type = "string"});

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = nxosNodeType.GetAllProperties();

            // Assert
            allProperties.Should().Contain(a => a.Key == "speed");
            allProperties.Should().Contain(a => a.Key == "storage");
        }

        [Test]
        public void GetAllProperties_Overrides_Properties_Of_Base_Node_Types()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string",
                Default = "10MBps",
                Required = true,
                Description = "switch description",
                Tags = new List<string>(new [] {"read_only"})
            } );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string",
                Default = "1GBps",
                Required = false,
                Description = "nxos description",
                Tags = new List<string>(new[] { "admin_only" })
            });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = nxosNodeType.GetAllProperties();

            // Assert
            allProperties["speed"].Default.Should().Be("1GBps");
            allProperties["speed"].Required.Should().BeFalse();
            allProperties["speed"].Description.Should().Be("nxos description");
            allProperties["speed"].Tags.ShouldAllBeEquivalentTo(new [] { "admin_only" });
        }

        [Test]
        public void Exception_Thrown_When_Type_On_Derived_NodeType_Differs_From_Base_NodeType()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string"
            } );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("speed", new ToscaProperty
            {
                Type = "integer"
            });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Act
            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults);
            
            // Assert
            validationResults.Should().Contain(r=>r.ErrorMessage.Equals("Property 'speed' has different type when overriden, which is not allowed"));
        }

        [Test]
        public void GetAllProperties_Does_Not_Override_Default_When_Not_Specified()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Properties.Add("speed", new ToscaProperty { Type = "string", Default = "10MBps", Required = true} );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("speed", new ToscaProperty { Type = "string", Required = false });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = nxosNodeType.GetAllProperties();

            // Assert
            allProperties["speed"].Default.Should().Be("10MBps");
            allProperties["speed"].Required.Should().BeFalse();
        }

        [Test]
        public void GetAllProperties_Does_Not_Override_Default_When_Not_Specified_When_Parsed_From_Yaml()
        {
            // Arrange
            var serviceTemplate = ToscaServiceTemplate.Load(@"
tosca_definitions_version: tosca_simple_yaml_1_0
metadata:
  template_author: Anonymous
  template_name: TOSCA
  template_version: 1.1
node_types:
  cloudshell.nodes.Switch:
    properties:
      speed:
        type: string
        required: true
        default: 10MBps
        constraints: 
          - valid_values: [10MBps, 100MBps, 1GBps]
  vendor.switch.NXOS:
    derived_from: cloudshell.nodes.Switch
    properties:
      speed:
        type: string
        default: 1GBps
".ToMemoryStream());

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = serviceTemplate.NodeTypes["vendor.switch.NXOS"].GetAllProperties();

            // Assert
            allProperties["speed"].Default.Should().Be("1GBps");
            var validValues = allProperties["speed"].GetConstraintsDictionary()["valid_values"];
            ((List<object>)validValues).ShouldBeEquivalentTo(new[] { "10MBps", "100MBps", "1GBps" });
        }

        [Test]
        public void GetAllProperties_Constraints_Overriden_On_Derived_Node_Type()
        {
            // Arrange
            var serviceTemplate = ToscaServiceTemplate.Load(@"
tosca_definitions_version: tosca_simple_yaml_1_0
metadata:
  template_author: Anonymous
  template_name: TOSCA
  template_version: 1.1
node_types:
  cloudshell.nodes.Switch:
    properties:
      speed:
        type: string
        required: true
        default: 10MBps
        constraints: 
          - valid_values: [10MBps, 100MBps, 1GBps]
  vendor.switch.NXOS:
    derived_from: cloudshell.nodes.Switch
    properties:
      speed:
        type: string
        default: 1mps
        constraints: 
          - valid_values: [1mps, 2mps, 3mps]
".ToMemoryStream());

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = serviceTemplate.NodeTypes["vendor.switch.NXOS"].GetAllProperties();

            // Assert
            allProperties["speed"].Default.Should().Be("1mps");
            var validValues = allProperties["speed"].GetConstraintsDictionary()["valid_values"];
            ((List<object>)validValues).ShouldBeEquivalentTo(new[] { "1mps", "2mps", "3mps" });
        }

        [Test]
        public void GetAllProperties_Constraints_Are_Merged_From_Base_And_Derived_Node_Type()
        {
            // Arrange
            var serviceTemplate = ToscaServiceTemplate.Load(@"
tosca_definitions_version: tosca_simple_yaml_1_0
metadata:
  template_author: Anonymous
  template_name: TOSCA
  template_version: 1.1
node_types:
  cloudshell.nodes.Switch:
    properties:
      speed:
        type: string
        required: true
        default: 10MBps
        constraints: 
          - valid_values: [10MBps, 100MBps, 1GBps]
  vendor.switch.NXOS:
    derived_from: cloudshell.nodes.Switch
    properties:
      speed:
        type: string
        default: 1mps
        constraints: 
          - max_length: 128
".ToMemoryStream());

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allProperties = serviceTemplate.NodeTypes["vendor.switch.NXOS"].GetAllProperties();

            // Assert
            allProperties["speed"].Default.Should().Be("1mps");
            var validValues = allProperties["speed"].GetConstraintsDictionary()["valid_values"];
            ((List<object>)validValues).ShouldBeEquivalentTo(new[] { "10MBps", "100MBps", "1GBps" });
            var maxLength = allProperties["speed"].GetConstraintsDictionary()["max_length"];
            maxLength.Should().Be("128");
        }

        [Test]
        public void GetAllRequirements_Return_Requirements_Of_Base_Node_Type()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.AddRequirement("internet", new ToscaRequirement { Capability = "wi-fi"});
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.AddRequirement("storage", new ToscaRequirement { Capability = "ssd"});

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allRequirements = nxosNodeType.GetAllRequirements();

            // Assert
            allRequirements.Should().ContainKey("internet");
            allRequirements.Should().ContainKey("storage");
        }

        [Test]
        public void GetAllRequirements_Shall_Not_Throw_Validation_Exception_When_Circular_Dependency_Exists()
        {
            // Arrange
            var portNodeType = new ToscaNodeType();
            portNodeType.AddRequirement("internet", new ToscaRequirement { Capability = "tosca.capabilities.Attachment", Node = "vendor.devices.Switch" });
            var switchNodetype = new ToscaNodeType();
            switchNodetype.AddRequirement("port", new ToscaRequirement { Capability = "tosca.capabilities.Attachment", Node = "vendor.parts.Port" });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("vendor.parts.Port", portNodeType);
            serviceTemplate.NodeTypes.Add("vendor.devices.Switch", switchNodetype);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Act
            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults);

            // Assert
            validationResults.Should().ContainSingle(r =>
                        r.ErrorMessage.Equals("Circular dependency detected by requirements on node type"));
        }

        [Test]
        public void Validation_Should_Not_Pass_When_Archive_With_Cyclic_Reference_Between_Requirements_Is_Loaded()
        {
            // Arrange
            var portNodeType = new ToscaNodeType();
            portNodeType.AddRequirement("internet", new ToscaRequirement { Capability = "tosca.capabilities.Attachment", Node = "vendor.devices.Switch" });
            var switchNodetype = new ToscaNodeType();
            switchNodetype.AddRequirement("port", new ToscaRequirement { Capability = "tosca.capabilities.Attachment", Node = "vendor.parts.Port" });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("vendor.parts.Port", portNodeType);
            serviceTemplate.NodeTypes.Add("vendor.devices.Switch", switchNodetype);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1,2,3),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            byte[] buffer;
            using (var memoryStream = new MemoryStream())
            {
                cloudServiceArchive.Save(memoryStream);
                memoryStream.Flush();
                buffer = memoryStream.GetBuffer();
            }

            // Act
            Action action = () => ToscaCloudServiceArchive.Load(new MemoryStream(buffer));
            
            // Assert
            action.ShouldThrow<ToscaValidationException>().WithMessage("Circular dependency detected by requirements on node type");
        }

        [Test]
        public void GetAllCapabilityTypes_Return_Capability_Types_Of_Base_Node_Type()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Capabilities.Add("internet", new ToscaCapability { Type = "capabilities.internet" });
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Capabilities.Add("storage", new ToscaCapability {Type = "capability.storage"});

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("cloudshell.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("vendor.switch.NXOS", nxosNodeType);
            serviceTemplate.CapabilityTypes.Add("capabilities.internet", new ToscaCapabilityType());
            serviceTemplate.CapabilityTypes.Add("capability.storage", new ToscaCapabilityType());

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CreatedBy = "Anonymous",
                CsarVersion = new Version(1, 1),
                EntryDefinitions = "tosca.yaml",
                ToscaMetaFileVersion = new Version(1, 1)
            });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            var validationResults = new List<ValidationResult>();
            cloudServiceArchive.TryValidate(out validationResults).Should()
                .BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            var allCapabilityTypes = nxosNodeType.GetAllCapabilityTypes();

            // Assert
            allCapabilityTypes.Should().HaveCount(3);
            allCapabilityTypes.Should().ContainKey("capability.storage");
            allCapabilityTypes.Should().ContainKey("capabilities.internet");
            allCapabilityTypes.Should().ContainKey("tosca.capabilities.Node");
        }
    }
}