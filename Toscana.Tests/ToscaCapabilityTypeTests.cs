using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaCapabilityTypeTests
    {
        [Test]
        public void Attributes_Should_Be_Empty_Upon_Initialization()
        {
            var toscaCapabilityType = new ToscaCapabilityType();

            toscaCapabilityType.Attributes.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            var toscaCapabilityType = new ToscaCapabilityType();

            toscaCapabilityType.Properties.Should().BeEmpty();
        }

        [Test]
        public void ValidSourceTypes_Should_Be_Empty_Upon_Initialization()
        {
            var toscaCapabilityType = new ToscaCapabilityType();

            toscaCapabilityType.ValidSourceTypes.Should().BeEmpty();
        }

        [Test]
        public void CapabilityTypeNotFoundException_Should_Be_Thrown_When_DerivedFrom_Capability_Does_Not_Exist()
        {
            var toscaCapabilityType = new ToscaCapabilityType();

            toscaCapabilityType.SetCloudServiceArchive(new ToscaCloudServiceArchive(new ToscaMetadata()));
            toscaCapabilityType.DerivedFrom = "base";

            // Act
            ToscaCapabilityType baseCapabilityType;
            Action action = () => baseCapabilityType = toscaCapabilityType.GetDerivedFromEntity();

            // Assert
            action.ShouldThrow<ToscaCapabilityTypeNotFoundException>().WithMessage("Capability type 'base' not found");
        }

        [Test]
        public void IsDerivedFrom_Returns_True_When_Derived_From_Another_Capability_Type()
        {
            // Arrange
            var derivedCapabilityType = new ToscaCapabilityType { DerivedFrom = "base"};
            var baseCapabilityType = new ToscaCapabilityType();

            var serviceTemplate = new ToscaServiceTemplate() { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.CapabilityTypes.Add("base", baseCapabilityType);
            serviceTemplate.CapabilityTypes.Add("derived", derivedCapabilityType);
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata { CreatedBy = "Anonymous", CsarVersion = new Version(1,1), EntryDefinitions = "tosca.yaml", ToscaMetaFileVersion = new Version(1,1)});
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults)
                .Should().BeTrue(string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage)));

            // Act
            // Assert
            derivedCapabilityType.IsDerivedFrom("base").Should().BeTrue();
        }

        [Test]
        public void GetAllProperties_Override_Base_Properties()
        {
            // Arrange
            var derivedCapabilityType = new ToscaCapabilityType { DerivedFrom = "base" };
            derivedCapabilityType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string",
                Required = false,
                Default = "10MBps",
                Description = "derived description"
            });
            var baseCapabilityType = new ToscaCapabilityType();
            baseCapabilityType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string",
                Required = true,
                Default = "",
                Description = "base description"
            });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.CapabilityTypes.Add("base", baseCapabilityType);
            serviceTemplate.CapabilityTypes.Add("derived", derivedCapabilityType);
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata { CreatedBy = "Anonymous", CsarVersion = new Version(1, 1), EntryDefinitions = "tosca.yaml", ToscaMetaFileVersion = new Version(1, 1) });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults)
                .Should().BeTrue(string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage)));

            // Act
            // Assert
            var speedProperty = derivedCapabilityType.GetAllProperties()["speed"];
            speedProperty.Type.Should().Be("string");
            speedProperty.Required.Should().BeFalse();
            speedProperty.Default.Should().Be("10MBps");
            speedProperty.Description.Should().Be("derived description");
        }

        [Test]
        public void GetAllProperties_Description_Should_Not_Be_Overriden_When_Another_Capability_Type_Overrides_It()
        {
            // Arrange
            var simpleCapabilityType = new ToscaCapabilityType { DerivedFrom = "base" };
            simpleCapabilityType.Properties.Add("list", new ToscaProperty()
            {
                Type = "list", Default = new object[] {}
            });

            var derivedCapabilityType = new ToscaCapabilityType { DerivedFrom = "base" };
            derivedCapabilityType.Properties.Add("description", new ToscaProperty
            {
                Type = "string",
                Default = "derived description"
            });
            var baseCapabilityType = new ToscaCapabilityType();
            baseCapabilityType.Properties.Add("description", new ToscaProperty
            {
                Type = "string",
                Default = "base description"
            });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.CapabilityTypes.Add("base", baseCapabilityType);
            serviceTemplate.CapabilityTypes.Add("derived", derivedCapabilityType);
            serviceTemplate.CapabilityTypes.Add("simple", simpleCapabilityType);
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata { CreatedBy = "Anonymous", CsarVersion = new Version(1, 1), EntryDefinitions = "tosca.yaml", ToscaMetaFileVersion = new Version(1, 1) });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults)
                .Should().BeTrue(string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage)));

            // Act
            var descriptionProperty = derivedCapabilityType.GetAllProperties()["description"];
            descriptionProperty = simpleCapabilityType.GetAllProperties()["description"];

            // Assert
            descriptionProperty.Default.Should().Be("base description");
        }

        [Test]
        public void GetAllProperties_Does_Not_Override_Base_Properties()
        {
            // Arrange
            var derivedCapabilityType = new ToscaCapabilityType { DerivedFrom = "base" };
            derivedCapabilityType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string"
            });
            var baseCapabilityType = new ToscaCapabilityType();
            baseCapabilityType.Properties.Add("speed", new ToscaProperty
            {
                Type = "string",
                Required = true,
                Default = "base default",
                Description = "base description"
            });

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.CapabilityTypes.Add("base", baseCapabilityType);
            serviceTemplate.CapabilityTypes.Add("derived", derivedCapabilityType);
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata { CreatedBy = "Anonymous", CsarVersion = new Version(1, 1), EntryDefinitions = "tosca.yaml", ToscaMetaFileVersion = new Version(1, 1) });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            List<ValidationResult> validationResults;
            cloudServiceArchive.TryValidate(out validationResults)
                .Should().BeTrue(string.Join(Environment.NewLine, validationResults.Select(r => r.ErrorMessage)));

            // Act
            var speedProperty = derivedCapabilityType.GetAllProperties()["speed"];

            // Assert
            speedProperty.Type.Should().Be("string");
            speedProperty.Required.Should().BeTrue();
            speedProperty.Default.Should().Be("base default");
            speedProperty.Description.Should().Be("base description");
        }

        [Test]
        public void GetAllProperties_Of_Root_Capability_Type_Is_Empty()
        {
            // Arrange
            var capabilityType = new ToscaCapabilityType();

            // Act
            var allProperties = capabilityType.GetAllProperties();

            // Assert
            allProperties.Should().BeEmpty();
        }

        [Test]
        public void GetAllProperties_Of_Capability_Type_Deriving_From_A_None_Existing_Capability_Type_Should_Throw_An_Exception()
        {
            // Arrange
            var capabilityType = new ToscaCapabilityType { DerivedFrom = "NOT_EXIST" };

            var serviceTemplate = new ToscaServiceTemplate { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
            serviceTemplate.CapabilityTypes.Add("my_cap_type", capabilityType);
            var cloudServiceArchive = new ToscaCloudServiceArchive(new ToscaMetadata { CreatedBy = "Anonymous", CsarVersion = new Version(1, 1), EntryDefinitions = "tosca.yaml", ToscaMetaFileVersion = new Version(1, 1) });
            cloudServiceArchive.AddToscaServiceTemplate("tosca.yaml", serviceTemplate);

            // Act
            Action action  = () => capabilityType.GetAllProperties();

            // Assert
            action.ShouldThrow<ToscaCapabilityTypeNotFoundException>().WithMessage("Capability type 'NOT_EXIST' not found");
        }
    }
}