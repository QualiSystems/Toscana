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

            toscaCapabilityType.SetToscaCloudServiceArchive(new ToscaCloudServiceArchive(new ToscaMetadata()));
            toscaCapabilityType.DerivedFrom = "base";

            // Act
            ToscaCapabilityType baseCapabilityType;
            Action action = () => baseCapabilityType = toscaCapabilityType.Base;

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
            derivedCapabilityType.Properties.Add("speed", new ToscaPropertyDefinition
            {
                Type = "string",
                Required = false,
                Default = "10MBps"
            });
            var baseCapabilityType = new ToscaCapabilityType();
            baseCapabilityType.Properties.Add("speed", new ToscaPropertyDefinition
            {
                Type = "integer",
                Required = true,
                Default = ""
            });

            var serviceTemplate = new ToscaServiceTemplate() { ToscaDefinitionsVersion = "tosca_simple_yaml_1_0" };
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