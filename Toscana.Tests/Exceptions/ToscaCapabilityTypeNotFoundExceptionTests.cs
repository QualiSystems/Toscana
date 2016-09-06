

using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaCapabilityTypeNotFoundExceptionTests
    {
        [Test]
        public void ToscaCapabilityTypeNotFoundException_Should_Be_Serializable()
        {
            // Act
            var toscaCapabilityTypeNotFoundException = new ToscaCapabilityTypeNotFoundException();

            // Assert
            toscaCapabilityTypeNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaCapabilityTypeNotFoundException__With_Message_Should_Be_Serializable()
        {
            // Act
            var toscaCapabilityTypeNotFoundException = new ToscaCapabilityTypeNotFoundException("message");

            // Assert
            toscaCapabilityTypeNotFoundException.Should().BeBinarySerializable();
        }
    }
}

