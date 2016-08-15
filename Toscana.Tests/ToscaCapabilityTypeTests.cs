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
    }
}