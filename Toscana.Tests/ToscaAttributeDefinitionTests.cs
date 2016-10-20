using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaAttributeDefinitionTests
    {
        [Test]
        public void Status_Should_Be_Supported_Upon_Initialization()
        {
            // Act
            var toscaNodeAttribute = new ToscaAttribute();

            // Assert
            toscaNodeAttribute.Status.Should().Be(ToscaPropertyStatus.supported);
        }
    }
}