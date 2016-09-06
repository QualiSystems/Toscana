using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaArtifactNotFoundExceptionTests
    {
        [Test]
        public void ArtifactNotFoundException_Should_Be_BinarySerializable()
        {
            var artifactNotFoundException = new ToscaArtifactNotFoundException();

            artifactNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ArtifactNotFoundException_With_Message_Should_Be_BinarySerializable()
        {
            var artifactNotFoundException = new ToscaArtifactNotFoundException("message");

            artifactNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ArtifactNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var artifactNotFoundException = new ToscaArtifactNotFoundException("message", innerException);

            artifactNotFoundException.InnerException.Message.Should().Be("inner");
            artifactNotFoundException.Message.Should().Be("message");
        }
    }
}