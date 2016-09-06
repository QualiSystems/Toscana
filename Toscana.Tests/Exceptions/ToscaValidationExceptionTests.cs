using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaValidationExceptionTests
    {
        [Test]
        public void ToscanaValidationException_Should_Be_Binary_Serializable()
        {
            var toscanaValidationException = new ToscaValidationException();

            toscanaValidationException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscanaValidationException_With_Message_Should_Be_Binary_Serializable()
        {
            var toscanaValidationException = new ToscaValidationException("message");

            toscanaValidationException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscanaValidationException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscanaValidationException = new ToscaValidationException("message", innerException);

            toscanaValidationException.InnerException.Message.Should().Be("inner");
            toscanaValidationException.Message.Should().Be("message");
        }
    }
}