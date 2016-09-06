using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaMetadataFileNotFoundExceptionTests
    {
        [Test]
        public void ToscaMetadataFileNotFound_Should_BeBinarySerializable()
        {
            var toscaMetadataFileNotFound = new ToscaMetadataFileNotFoundException();

            toscaMetadataFileNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaMetadataFileNotFound_With_Message_Should_BeBinarySerializable()
        {
            var toscaMetadataFileNotFound = new ToscaMetadataFileNotFoundException("message");

            toscaMetadataFileNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaMetadataFileNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaMetadataFileNotFoundException = new ToscaMetadataFileNotFoundException("message", innerException);

            toscaMetadataFileNotFoundException.InnerException.Message.Should().Be("inner");
            toscaMetadataFileNotFoundException.Message.Should().Be("message");
        }
    }
}