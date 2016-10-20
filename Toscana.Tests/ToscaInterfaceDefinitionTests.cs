

using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaInterfaceDefinitionTests
    {
        [Test]
        public void Inputs_Should_Be_Empty_Upon_Initialization()
        {
            // Act
            var toscaInterfaceDefinition = new ToscaInterface();

            // Assert
            toscaInterfaceDefinition.Inputs.Should().BeEmpty();
        }
    }
}

