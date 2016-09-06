using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaNodeTypeNotFoundExceptionTests
    {
        [Test]
        public void ToscaNodeTypeNotFoundException_Should_BeBinarySerializable()
        {
            var toscaNodeTypeNotFoundException = new ToscaNodeTypeNotFoundException();

            toscaNodeTypeNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaNodeTypeNotFoundException_With_Message_Should_BeBinarySerializable()
        {
            var toscaNodeTypeNotFoundException = new ToscaNodeTypeNotFoundException("message");

            toscaNodeTypeNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaNodeTypeNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaNodeTypeNotFoundException = new ToscaNodeTypeNotFoundException("message", innerException);

            toscaNodeTypeNotFoundException.InnerException.Message.Should().Be("inner");
            toscaNodeTypeNotFoundException.Message.Should().Be("message");
        }
    }
}