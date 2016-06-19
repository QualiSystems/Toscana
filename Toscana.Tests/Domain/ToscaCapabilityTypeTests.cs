using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests.Domain
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
    }
}