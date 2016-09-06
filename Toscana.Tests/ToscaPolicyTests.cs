using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaPolicyTests
    {
        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            var toscaPolicy = new ToscaPolicy();

            toscaPolicy.Properties.Should().BeEmpty();
        }

        [Test]
        public void Targets_Should_Be_Empty_Upon_Initialization()
        {
            var toscaPolicy = new ToscaPolicy();

            toscaPolicy.Targets.Should().BeEmpty();
        }
    }
}