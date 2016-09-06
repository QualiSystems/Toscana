

using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaArtifactTypeTests
    {
        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            var toscaArtifactType = new ToscaArtifactType();

            toscaArtifactType.Properties.Should().BeEmpty();
        }
    }
}

