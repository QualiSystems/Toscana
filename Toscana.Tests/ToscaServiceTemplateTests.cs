using System;
using FluentAssertions;
using NUnit.Framework;
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
    }
}