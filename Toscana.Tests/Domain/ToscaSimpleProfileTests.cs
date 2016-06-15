using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaSimpleProfileTests
    {
        [Test]
        public void Ctor_NodeTypes_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaSimpleProfile();

            toscaSimpleProfile.NodeTypes.Should().NotBeNull();
            toscaSimpleProfile.NodeTypes.Should().HaveCount(0);
        }

        [Test]
        public void Ctor_CapabilityTypes_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaSimpleProfile();

            toscaSimpleProfile.CapabilityTypes.Should().NotBeNull();
            toscaSimpleProfile.CapabilityTypes.Should().HaveCount(0);
        }

        [Test]
        public void Ctor_Imports_Initialized_To_Empty()
        {
            var toscaSimpleProfile = new ToscaSimpleProfile();

            toscaSimpleProfile.Imports.Should().NotBeNull();
            toscaSimpleProfile.Imports.Should().HaveCount(0);
        }

        [Test]
        public void ToscaDefinitionsVersion_Invalid_ValidationExceptionThrown()
        {
            // Arrange
            var toscaSimpleProfile = new ToscaSimpleProfile
            {
                ToscaDefinitionsVersion = "INVALID"
            };

            var toscaValidator = new ToscaValidator();

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
            var toscaSimpleProfile = new ToscaSimpleProfile
            {
                ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
            };

            var toscaValidator = new ToscaValidator();

            // Act
            Action action = () => toscaValidator.Validate(toscaSimpleProfile);

            // Assert
            action.ShouldNotThrow<Exception>();
        }
    }
}