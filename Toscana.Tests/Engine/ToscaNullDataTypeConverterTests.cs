using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaNullDataTypeConverterTests
    {
        [Test]
        public void Can_Convert_Should_Return_True_For_Null()
        {
            // Arrange
            var toscaNullDataTypeConverter = new ToscaNullDataTypeConverter();

            // Act
            var canConvert = toscaNullDataTypeConverter.CanConvert("null");

            // Assert
            canConvert.Should().BeTrue();
        }

        [Test]
        public void Can_Convert_Should_Return_True_For_Other_Type()
        {
            // Arrange
            var toscaNullDataTypeConverter = new ToscaNullDataTypeConverter();

            // Act
            var canConvert = toscaNullDataTypeConverter.CanConvert("some other type");

            // Assert
            canConvert.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_True_For_Anything()
        {
            // Arrange
            var toscaNullDataTypeConverter = new ToscaNullDataTypeConverter();

            // Act
            object result;
            var canConvert = toscaNullDataTypeConverter.TryParse("some other type", out result);

            // Assert
            canConvert.Should().BeTrue();
        }
    }
}