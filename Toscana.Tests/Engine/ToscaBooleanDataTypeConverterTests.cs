using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaBooleanDataTypeConverterTests
    {
        [Test]
        public void CanConvert_Should_Be_True_For_Boolean()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            var canConvert = toscaBooleanDataTypeConverter.CanConvert("boolean");

            // Assert
            canConvert.Should().BeTrue();
        }

        [Test]
        public void CanConvert_Should_Be_False_For_Other_Types()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            var canConvert = toscaBooleanDataTypeConverter.CanConvert("some other type");

            // Assert
            canConvert.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_True_For_True()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaBooleanDataTypeConverter.TryParse("true", out result);

            // Assert
            tryParse.Should().BeTrue();
        }

        [Test]
        public void TryParse_Should_Return_True_For_False()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaBooleanDataTypeConverter.TryParse("false", out result);

            // Assert
            tryParse.Should().BeTrue();
        }

        [Test]
        public void TryParse_Should_Return_False_For_Something_Else()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaBooleanDataTypeConverter.TryParse("something else", out result);

            // Assert
            tryParse.Should().BeFalse();
        }

        [Test]
        public void TryParse_Should_Return_False_For_Null()
        {
            // Arrange
            var toscaBooleanDataTypeConverter = new ToscaBooleanDataTypeConverter();

            // Act
            object result;
            var tryParse = toscaBooleanDataTypeConverter.TryParse(null, out result);

            // Assert
            tryParse.Should().BeFalse();
        }
    }
}