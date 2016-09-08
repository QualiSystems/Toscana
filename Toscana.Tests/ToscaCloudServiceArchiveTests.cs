using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
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
            toscaCloudServiceArchive.GetEntryPointServiceTemplate().Description.Should().Be("tosca1 description");
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
                EntryDefinitions = "tosca.yaml"
            };

            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);

            // Act
            toscaCloudServiceArchive.AddArtifact("some_icon.png", "IMAGE".ToByteArray(Encoding.ASCII));

            // Assert
            toscaCloudServiceArchive.GetArtifactBytes("some_icon.png")
                .Should()
                .BeEquivalentTo(new byte[] {73, 77, 65, 71, 69});
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
                File = "device.png",
                Type = "image"
            });
            var toscaServiceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" } ;
            toscaServiceTemplate.NodeTypes.Add("device", toscaNodeType);
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                EntryDefinitions = "tosca.yaml", CreatedBy = "Anonymous", CsarVersion = new Version(1,1), ToscaMetaFileVersion = new Version(1,1)
            });

            // Act
            toscaCloudServiceArchive.AddToscaServiceTemplate("definition", toscaServiceTemplate);
            List<ValidationResult> validationResults;
            toscaCloudServiceArchive.TryValidate(out validationResults);

            // Assert
            validationResults.Select(a=>a.ErrorMessage).ToArray()
                .ShouldAllBeEquivalentTo(new[] { "Artifact 'device.png' not found in Cloud Service Archive." });

            //action.ShouldThrow<ToscaArtifactNotFoundException>()
            //    .WithMessage("Artifact 'device.png' not found in Cloud Service Archive.");
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
            });

            // Act
            Action action = () => toscaCloudServiceArchive.GetArtifactBytes("NOT_EXISTING.png");

            // Assert
            action.ShouldThrow<ToscaArtifactNotFoundException>()
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
        public void GetArtifactBytes_OnCloudServiceArchive_Without_Artifacts_Should_Throw_ArtifactNotFoundException()
        {
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            Action action = () => toscaCloudServiceArchive.GetArtifactBytes("not_existing_file.png");

            action.ShouldThrow<ToscaArtifactNotFoundException>()
                .WithMessage("Artifact 'not_existing_file.png' not found in Cloud Service Archive.");
        }

        [Test]
        public void Base_Property_Set_To_NodeType_Instance_Of_Derived_From()
        {
            // Arrange
            var deviceNodeType = new ToscaNodeType();
            deviceNodeType.Properties.Add("vendor", new ToscaPropertyDefinition { Type = "string" });

            var switchNodeType = new ToscaNodeType { DerivedFrom = "tosca.nodes.Device" };
            switchNodeType.Properties.Add("speed", new ToscaPropertyDefinition { Type = "integer" });

            var serviceTemplate = new ToscaServiceTemplate {ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"};
            serviceTemplate.NodeTypes.Add("tosca.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.Device", deviceNodeType);

            // Act
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Assert
            switchNodeType.Base.Properties.Should().ContainKey("vendor");
        }

        [Test]
        public void Base_Property_Of_Root_NodeType_Should_Be_Null()
        {
            // Arrange
            var deviceNodeType = new ToscaNodeType();
            deviceNodeType.Properties.Add("vendor", new ToscaPropertyDefinition { Type = "string" });

            var switchNodeType = new ToscaNodeType { DerivedFrom = "tosca.nodes.Device" };
            switchNodeType.Properties.Add("speed", new ToscaPropertyDefinition { Type = "integer" });

            var serviceTemplate = new ToscaServiceTemplate {ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.NodeTypes.Add("tosca.nodes.Switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.Device", deviceNodeType);

            // Act
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Assert
            deviceNodeType.DerivedFrom.Should().Be(ToscaDefaults.ToscaNodesRoot);
            deviceNodeType.Base.Attributes.Should().ContainKey("tosca_id", "tosca.nodes.Root has tosca_id property");
        }

        [Test]
        public void Base_Property_Of_Capability_Type_Is_Capability_Of_Derived_From()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate();
            var basicCapabilityType = new ToscaCapabilityType();
            basicCapabilityType.Properties.Add("username", new ToscaPropertyDefinition { Type = "string"});
            serviceTemplate.CapabilityTypes.Add("basic", basicCapabilityType);
            serviceTemplate.CapabilityTypes.Add("connectable", new ToscaCapabilityType
            {
                DerivedFrom = "basic"
            });

            // Act
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            // Assert
            cloudServiceArchive.CapabilityTypes["connectable"].Base.Properties.Should().ContainKey("username");
        }

        [Test]
        public void Node_Type_Without_Derived_From_Should_Have_Root_As_Their_Derived_From()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate();
            serviceTemplate.NodeTypes.Add("device", new ToscaNodeType());

            // Act
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            // Assert
            cloudServiceArchive.NodeTypes["device"].DerivedFrom.Should().Be("tosca.nodes.Root");
            cloudServiceArchive.NodeTypes["device"].Base.Should().NotBeNull();
        }

        [Test]
        public void Capability_Type_Without_Derived_From_Should_Have_Root_As_Their_Derived_From()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate();
            serviceTemplate.CapabilityTypes.Add("connectable", new ToscaCapabilityType());

            // Act
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            // Assert
            cloudServiceArchive.CapabilityTypes["connectable"].DerivedFrom.Should().Be("tosca.capabilities.Root");
            cloudServiceArchive.CapabilityTypes["connectable"].Base.Should().NotBeNull();
        }

        [Test]
        public void TraverseNodeTypesInheritance_Traverses_Nodes_From_Root_To_Its_Derived_Node_Types()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate();
            serviceTemplate.NodeTypes.Add("device", new ToscaNodeType());
            serviceTemplate.NodeTypes.Add("switch", new ToscaNodeType { DerivedFrom = "device"});
            serviceTemplate.NodeTypes.Add("router", new ToscaNodeType { DerivedFrom = "device"});

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata());

            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);
            
            // Act
            var discoveredNodeTypeNames = new List<string>();
            cloudServiceArchive.TraverseNodeTypesInheritance((nodeTypeName, nodeType) => { discoveredNodeTypeNames.Add(nodeTypeName );});

            // Assert
            discoveredNodeTypeNames.ShouldBeEquivalentTo(new[] { "tosca.nodes.Root", "device", "switch", "router" });
        }

        [Test]
        public void TraverseNodeTypesByRequirements_Traverses_Nodes_From_Specific_Node_Type_By_It_Requirements()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate{ ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };

            var powerNodeType = new ToscaNodeType();
            powerNodeType.AddRequirement("power.cable", new ToscaRequirement { Node = "tosca.nodes.cable", Capability = "cable" });
            powerNodeType.AddRequirement("power.switch", new ToscaRequirement { Node = "tosca.nodes.switch", Capability = "cable" });

            var deviceNodeType = new ToscaNodeType();
            deviceNodeType.AddRequirement("power", new ToscaRequirement { Node = "tosca.nodes.power", Capability = "power"} );

            var cableNodeType = new ToscaNodeType();

            var switchNodeType = new ToscaNodeType();

            serviceTemplate.NodeTypes.Add("tosca.nodes.mic", new ToscaNodeType());
            serviceTemplate.NodeTypes.Add("tosca.nodes.cable", cableNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.switch", switchNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.power", powerNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.device", deviceNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CsarVersion = new Version(1,1),
                EntryDefinitions = "sample.yaml",
                ToscaMetaFileVersion = new Version(1,1),
                CreatedBy = "Anonymous"
            });
            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults);
            var validationErrors = string.Join(Environment.NewLine, validationResults.Select(a => a.ErrorMessage).ToArray());
            validationErrors.Should().BeEmpty();

            // Act
            var discoveredNodeTypeNames = new List<string>();
            cloudServiceArchive.TraverseNodeTypesByRequirements("tosca.nodes.device", (nodeTypeName, nodeType) => { discoveredNodeTypeNames.Add(nodeTypeName); });

            // Assert
            discoveredNodeTypeNames.ShouldBeEquivalentTo(new[] { "tosca.nodes.device", "tosca.nodes.power", "tosca.nodes.switch", "tosca.nodes.cable" });
        }

        [Test]
        public void TraverseNodeTypesByRequirements_Traverses_Nodes_From_Specific_Node_Type_By_Requirements_Of_Its_Base_Node_Type()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate{ ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };

            var deviceNodeType = new ToscaNodeType();
            deviceNodeType.AddRequirement("power", new ToscaRequirement { Node = "tosca.nodes.port", Capability = "port"} );

            var switchNodeType = new ToscaNodeType { DerivedFrom = "tosca.nodes.device" };

            serviceTemplate.NodeTypes.Add("tosca.nodes.port", new ToscaNodeType());
            serviceTemplate.NodeTypes.Add("tosca.nodes.device", deviceNodeType);
            serviceTemplate.NodeTypes.Add("tosca.nodes.switch", switchNodeType);

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CsarVersion = new Version(1,1),
                EntryDefinitions = "sample.yaml",
                ToscaMetaFileVersion = new Version(1,1),
                CreatedBy = "Anonymous"
            });
            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults)
                .Should().BeTrue(string.Join(Environment.NewLine, validationResults.Select(a => a.ErrorMessage)));

            // Act
            var discoveredNodeTypeNames = new List<string>();
            cloudServiceArchive.TraverseNodeTypesByRequirements("tosca.nodes.switch", (nodeTypeName, nodeType) => { discoveredNodeTypeNames.Add(nodeTypeName); });

            // Assert
            discoveredNodeTypeNames.ShouldBeEquivalentTo(new[] { "tosca.nodes.port", "tosca.nodes.switch" });
        }

        [Test]
        public void TraverseNodeTypesByRequirements_Should_Throw_Exception_When_NodeType_ToStart_NotFound()
        {
            // Arrange
            var serviceTemplate = new ToscaServiceTemplate{ ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };

            serviceTemplate.NodeTypes.Add("tosca.nodes.device", new ToscaNodeType());

            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata
            {
                CsarVersion = new Version(1,1),
                EntryDefinitions = "not_existing.yaml",
                ToscaMetaFileVersion = new Version(1,1),
                CreatedBy = "Anonymous"
            });
            cloudServiceArchive.AddToscaServiceTemplate("sample.yaml", serviceTemplate);

            // Act
            Action action = () => cloudServiceArchive.TraverseNodeTypesByRequirements("NOT_EXISTING", (nodeTypeName, nodeType) => { });

            // Assert
            action.ShouldThrow<ToscaNodeTypeNotFoundException>().WithMessage("Node type 'NOT_EXISTING' not found");
        }

        [Test]
        public void It_Should_Be_Possible_To_Save_And_Load_Cloud_Service_Archive()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1,1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1,0);
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" });

            List<ValidationResult> results;
            cloudServiceArchive.TryValidate(out results).Should().BeTrue(string.Join(Environment.NewLine, results.Select(r=>r.ErrorMessage)));

            using (var memoryStream = new MemoryStream())
            {
                cloudServiceArchive.Save(memoryStream);

                var serviceArchive = ToscaCloudServiceArchive.Load(memoryStream);

                // Assert
                serviceArchive.ToscaMetadata.CreatedBy.Should().Be("Anonymous");
            }
        }

        [Test]
        public void It_Should_Be_Possible_To_Save_And_Load_Cloud_Service_Archive_With_Artifacts()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1,1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1,0);
            cloudServiceArchive.AddArtifact("readme.txt", "readme content".ToByteArray(Encoding.ASCII));
            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            var nodeType = new ToscaNodeType();
            nodeType.Artifacts.Add("readme", new ToscaArtifact { Type = "tosca.artifacts.File", File = "readme.txt" });
            serviceTemplate.NodeTypes.Add("some_node", nodeType);
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", 
                serviceTemplate);

            List<ValidationResult> results;
            cloudServiceArchive.TryValidate(out results).Should().BeTrue(string.Join(Environment.NewLine, results.Select(r=>r.ErrorMessage)));

            using (var memoryStream = new MemoryStream())
            {
                cloudServiceArchive.Save(memoryStream);

                var serviceArchive = ToscaCloudServiceArchive.Load(memoryStream);

                // Assert
                serviceArchive.ToscaMetadata.CreatedBy.Should().Be("Anonymous");
                serviceArchive.GetArtifactBytes("readme.txt")
                    .Should()
                    .BeEquivalentTo("readme content".ToByteArray(Encoding.ASCII));
            }
        }

        [Test]
        public void ToscaServiceTemplateAlreadyExistsException_Should_Be_Thrown_When_Same_Service_Template_Added_Twice()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1, 1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1, 0);

            cloudServiceArchive.AddToscaServiceTemplate("reference1.yml", new ToscaServiceTemplate());
            Action action = () => cloudServiceArchive.AddToscaServiceTemplate("reference1.yml", new ToscaServiceTemplate());

            action.ShouldThrow<ToscaServiceTemplateAlreadyExistsException>()
                .WithMessage("Service Template 'reference1.yml' already exists");
        }

        [Test]
        public void Exception_Should_Be_Thrown_When_Complex_Data_Type_Consists_Of_Not_Existing_Type()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1, 1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1, 0);

            var toscaAsString = @"
tosca_definitions_version: tosca_simple_yaml_1_0
data_types:
  tosca.datatypes.Complex:
    properties:
      real:
        type: weight";

            using (var memoryStream = toscaAsString.ToMemoryStream())
            {
                // Act
                var serviceTemplate = ToscaServiceTemplate.Parse(memoryStream);
                cloudServiceArchive.AddToscaServiceTemplate("tosca.yml", serviceTemplate);

                List<ValidationResult> results;
                cloudServiceArchive.TryValidate(out results);

                results.Should().HaveCount(1);
                results.Should().Contain(a => a.ErrorMessage.Contains("Data type 'weight' specified as part of data type 'tosca.datatypes.Complex' not found."));
            }
        }

        [Test]
        public void Exception_Should_Be_Thrown_When_Valid_Values_Are_Not_Of_Compatible_Type()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1, 1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1, 0);

            var toscaAsString = @"
tosca_definitions_version: tosca_simple_yaml_1_0
node_types:
  tosca.nodes.MyNode:
    properties:
      ports_number:
        type: integer
        constraints:
          - valid_values: [a, 1]
";

            using (var memoryStream = toscaAsString.ToMemoryStream())
            {
                // Act
                Action action = () => ToscaServiceTemplate.Parse(memoryStream);

                // Assert
                action.ShouldThrow<ToscaValidationException>()
                    .WithMessage(
                        "Value 'a' of constraint 'a,1' cannot be parsed according to property data type 'integer'");
            }
        }

        [Test]
        public void Validation_Shall_Pass_When_Complex_Data_Type_Consists_Of_Built_In_Type()
        {
            var cloudServiceArchive = new ToscaCloudServiceArchive();
            cloudServiceArchive.ToscaMetadata.CreatedBy = "Anonymous";
            cloudServiceArchive.ToscaMetadata.CsarVersion = new Version(1, 1);
            cloudServiceArchive.ToscaMetadata.EntryDefinitions = "tosca.yaml";
            cloudServiceArchive.ToscaMetadata.ToscaMetaFileVersion = new Version(1, 0);

            var toscaAsString = @"
tosca_definitions_version: tosca_simple_yaml_1_0
data_types:
  tosca.datatypes.Complex:
    properties:
      real:
        type: integer";

            using (var memoryStream = toscaAsString.ToMemoryStream())
            {
                // Act
                var serviceTemplate = ToscaServiceTemplate.Parse(memoryStream);
                cloudServiceArchive.AddToscaServiceTemplate("tosca.yml", serviceTemplate);

                // Assert
                List<ValidationResult> results;
                cloudServiceArchive.TryValidate(out results).Should().BeTrue(string.Join(Environment.NewLine, results));
            }
        }
    }
}