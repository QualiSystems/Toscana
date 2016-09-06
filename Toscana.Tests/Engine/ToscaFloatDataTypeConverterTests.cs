using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaFloatDataTypeConverterTests
    {
        [Test]
        public void CanConvert_Should_Be_True_For_Float()
        {
            // Arrange
            var toscaFloatDataTypeConverter = new ToscaFloatDataTypeConverter();

            // Act
            var canConvert = toscaFloatDataTypeConverter.CanConvert("float");

            // Assert
            canConvert.Should().BeTrue();
        }

        [Test]
        public void CanConvert_Should_Be_False_For_Other_Types()
        {
            // Arrange
            var toscaFloatDataTypeConverter = new ToscaFloatDataTypeConverter();

            // Act
            var canConvert = toscaFloatDataTypeConverter.CanConvert("other data type");

            // Assert
            canConvert.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Be_True_For_Valid_Float()
        {
            // Arrange
            var toscaFloatDataTypeConverter = new ToscaFloatDataTypeConverter();

            // Act
            object result;
            var canConvert = toscaFloatDataTypeConverter.TryParse(3.1, out result);

            // Assert
            canConvert.Should().BeTrue();
        }

        [Test]
        public void TryParse_Should_Be_False_For_Invalid_Float()
        {
            // Arrange
            var toscaFloatDataTypeConverter = new ToscaFloatDataTypeConverter();

            // Act
            object result;
            var canConvert = toscaFloatDataTypeConverter.TryParse("invalid float", out result);

            // Assert
            canConvert.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_False_For_Null()
        {
            // Arrange
            var floatDataTypeConverter = new ToscaFloatDataTypeConverter();

            // Act
            object result;
            var tryParse = floatDataTypeConverter.TryParse(null, out result);

            // Assert
            tryParse.Should().BeFalse();
        }

    }
}