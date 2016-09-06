

using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaRelationshipTypeTests
    {
        [Test]
        public void Attributes_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaRelationshipType = new ToscaRelationshipType();

            // Assert
            toscaRelationshipType.Attributes.Should().BeEmpty();
        }

        [Test]
        public void Interfaces_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaRelationshipType = new ToscaRelationshipType();

            // Assert
            toscaRelationshipType.Interfaces.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaRelationshipType = new ToscaRelationshipType();

            // Assert
            toscaRelationshipType.Properties.Should().BeEmpty();
        }

        [Test]
        public void ValidTargetTypes_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaRelationshipType = new ToscaRelationshipType();

            // Assert
            toscaRelationshipType.ValidTargetTypes.Should().BeEmpty();
        }
    }
}

