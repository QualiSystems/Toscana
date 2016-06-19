using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaNodeAttributeTests
    {
        [Test]
        public void Status_Should_Be_Supported_Upon_Initialization()
        {
            // Act
            var toscaNodeAttribute = new ToscaNodeAttribute();

            // Assert
            toscaNodeAttribute.Status.Should().Be(ToscaPropertyStatus.supported);
        }
    }
}