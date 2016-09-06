using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaDataTypeNotFoundExceptionTests
    {
        [Test]
        public void ToscaDataTypeNotFoundException__With_Message_Should_Be_Binary_Serializable()
        {
            var toscaDataTypeNotFoundException = new ToscaDataTypeNotFoundException("message");

            // Assert
            toscaDataTypeNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaDataTypeNotFoundException_Should_Be_Binary_Serializable()
        {
            var toscaDataTypeNotFoundException = new ToscaDataTypeNotFoundException();

            // Assert
            toscaDataTypeNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaDataTypeNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaDataTypeNotFoundException = new ToscaDataTypeNotFoundException("message", innerException);

            toscaDataTypeNotFoundException.InnerException.Message.Should().Be("inner");
            toscaDataTypeNotFoundException.Message.Should().Be("message");
        }
    }
}