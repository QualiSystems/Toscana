

using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaInvalidFileExceptionTests
    {
        [Test]
        public void ToscaInvalidFileException_Should_Be_Binary_Serializable()
        {
            var toscaInvalidFileException = new ToscaInvalidFileException();

            toscaInvalidFileException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaInvalidFileException_With_Message_Should_Be_Binary_Serializable()
        {
            var toscaInvalidFileException = new ToscaInvalidFileException("message");

            toscaInvalidFileException.Should().BeBinarySerializable();
        }
        
        [Test]
        public void ToscaInvalidFileException_With_Message_And_Inner_Exception_Should_Be_Binary_Serializable()
        {
            var toscaInvalidFileException = new ToscaInvalidFileException("message", new InvalidDataException("inner message"));

            toscaInvalidFileException.Should().BeBinarySerializable();
        }
    }
}

