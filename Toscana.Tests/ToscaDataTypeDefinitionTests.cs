using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaDataTypeDefinitionTests
    {
        [Test]
        public void Properties_Should_Be_Empty_Upon_Initiaization()
        {
            // Act
            var dataTypeDefinition = new ToscaDataTypeDefinition();

            // Assert
            dataTypeDefinition.Properties.Should().BeEmpty();
        }
    }
}