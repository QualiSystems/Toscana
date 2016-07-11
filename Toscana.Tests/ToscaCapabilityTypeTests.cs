using System;
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
    }
}