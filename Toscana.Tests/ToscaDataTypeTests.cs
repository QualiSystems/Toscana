using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaDataTypeTests
    {
        [Test]
        public void Properties_Should_Be_Empty_Upon_Initiaization()
        {
            // Act
            var dataTypeDefinition = new ToscaDataType();

            // Assert
            dataTypeDefinition.Properties.Should().BeEmpty();
        }
    }
}