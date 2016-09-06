using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaGroupTests
    {
        [Test]
        public void Interfaces_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaGroup = new ToscaGroup();

            // Assert
            toscaGroup.Interfaces.Should().BeEmpty();
        }

        [Test]
        public void Members_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaGroup = new ToscaGroup();

            // Assert
            toscaGroup.Members.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaGroup = new ToscaGroup();

            // Assert
            toscaGroup.Properties.Should().BeEmpty();
        }
    }
}