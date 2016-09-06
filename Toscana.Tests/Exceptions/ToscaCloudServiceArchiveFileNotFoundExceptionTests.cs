using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaCloudServiceArchiveFileNotFoundExceptionTests
    {
        [Test]
        public void ToscaCloudServiceArchiveFileNotFoundException_Should_BeBinarySerializable()
        {
            var toscaCloudServiceArchiveFileNotFoundException = new ToscaCloudServiceArchiveFileNotFoundException();

            toscaCloudServiceArchiveFileNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaCloudServiceArchiveFileNotFoundException_With_Message_Should_BeBinarySerializable()
        {
            var toscaCloudServiceArchiveFileNotFoundException =
                new ToscaCloudServiceArchiveFileNotFoundException("message");

            toscaCloudServiceArchiveFileNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaCloudServiceArchiveFileNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaCloudServiceArchiveFileNotFoundException = new ToscaCloudServiceArchiveFileNotFoundException("message", innerException);

            toscaCloudServiceArchiveFileNotFoundException.InnerException.Message.Should().Be("inner");
            toscaCloudServiceArchiveFileNotFoundException.Message.Should().Be("message");
        }
    }
}