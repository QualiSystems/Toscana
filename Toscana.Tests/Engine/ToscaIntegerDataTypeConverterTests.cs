using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaIntegerDataTypeConverterTests
    {
        [Test]
        public void CanConvert_Should_Return_True_For_Integer()
        {
            // Arrange
            var toscaIntegerDataTypeConverter = new ToscaIntegerDataTypeConverter();

            // Act
            var canConvert = toscaIntegerDataTypeConverter.CanConvert("integer");

            // Assert
            canConvert.Should().BeTrue();
        }

        [Test]
        public void CanConvert_Should_Return_False_For_Something_Else()
        {
            // Arrange
            var toscaIntegerDataTypeConverter = new ToscaIntegerDataTypeConverter();

            // Act
            var canConvert = toscaIntegerDataTypeConverter.CanConvert("something else");

            // Assert
            canConvert.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_False_For_Null()
        {
            // Arrange
            var toscaIntegerDataTypeConverter = new ToscaIntegerDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaIntegerDataTypeConverter.TryParse(null, out result);

            // Assert
            tryParse.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_False_For_Something_Else()
        {
            // Arrange
            var toscaIntegerDataTypeConverter = new ToscaIntegerDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaIntegerDataTypeConverter.TryParse("is not an integer", out result);

            // Assert
            tryParse.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_True_For_Integer()
        {
            // Arrange
            var toscaIntegerDataTypeConverter = new ToscaIntegerDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaIntegerDataTypeConverter.TryParse(22, out result);

            // Assert
            tryParse.Should().BeTrue();
        }
    }
}