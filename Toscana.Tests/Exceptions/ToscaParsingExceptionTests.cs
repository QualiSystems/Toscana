using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaParsingExceptionTests
    {
        [Test]
        public void ToscaParsingException_Should_Be_Binary_Serializable()
        {
            var toscaParsingException = new ToscaParsingException();

            toscaParsingException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaParsingException_With_Message_Should_Be_Binary_Serializable()
        {
            var toscaParsingException = new ToscaParsingException("message");

            toscaParsingException.Should().BeBinarySerializable();
        }
    }
}