using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaCapabilityAssignmentTests
    {
        [Test]
        public void Attributes_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaCapabilityAssignment = new ToscaCapabilityAssignment();

            // Assert
            toscaCapabilityAssignment.Attributes.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaCapabilityAssignment = new ToscaCapabilityAssignment();

            // Assert
            toscaCapabilityAssignment.Properties.Should().BeEmpty();
        }
    }
}