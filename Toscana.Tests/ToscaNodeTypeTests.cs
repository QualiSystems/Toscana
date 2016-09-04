using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

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
            var validationSuccess = cloudServiceArchive.TryValidate(out validationResults);

            // Assert
            validationSuccess.Should().BeFalse();
            validationResults.Should()
                .Contain(r => r.ErrorMessage.Contains("Node type 'cloudshell.nodes.Switch' not found"));
        }

        [Test]
        public void GetAllProperties_Return_Properties_Of_Base_Node_Types()
        {
            // Arrange
            var switchNodeType = new ToscaNodeType();
            switchNodeType.Properties.Add("speed",new ToscaPropertyDefinition { Type = "string"} );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("storage", new ToscaPropertyDefinition { Type = "string"});

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
            switchNodeType.Properties.Add("speed", new ToscaPropertyDefinition { Type = "string", Default = "10MBps", Required = true} );
            var nxosNodeType = new ToscaNodeType { DerivedFrom = "cloudshell.nodes.Switch" };
            nxosNodeType.Properties.Add("speed", new ToscaPropertyDefinition { Type = "string", Default = "1GBps", Required = false });

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