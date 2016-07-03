using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaTopologyInputTests
    {
        [Test]
        public void Constraints_Should_Be_Empty_Upon_Initialization()
        {
            var toscaTopologyInput = new ToscaTopologyInput();

            toscaTopologyInput.Constraints.Should().BeEmpty();
        }
    }
}