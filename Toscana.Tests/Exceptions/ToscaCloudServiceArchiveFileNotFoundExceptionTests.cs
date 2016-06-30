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
    }
}