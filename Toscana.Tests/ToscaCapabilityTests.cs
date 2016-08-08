using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaCapabilityTests
    {
        [Test]
        public void Properties_Are_Not_Null_Upon_Initialization()
        {
            var toscaCapability = new ToscaCapability();

            // Assert
            toscaCapability.Properties.Should().NotBeNull();
        }

        [Test]
        public void Attributes_Are_Not_Null_Upon_Initialization()
        {
            var toscaCapability = new ToscaCapability();

            // Assert
            toscaCapability.Attributes.Should().NotBeNull();
        }
    }
}