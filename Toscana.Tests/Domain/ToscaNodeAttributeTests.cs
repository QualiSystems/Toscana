using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaNodeAttributeTests
    {
        [Test]
        public void Status_Should_Be_Supported_Upon_Initialization()
        {
            // Act
            var toscaNodeAttribute = new ToscaAttributeDefinition();

            // Assert
            toscaNodeAttribute.Status.Should().Be(ToscaPropertyStatus.supported);
        }
    }
}