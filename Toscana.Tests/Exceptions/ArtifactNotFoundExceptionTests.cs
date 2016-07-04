using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ArtifactNotFoundExceptionTests
    {
        [Test]
        public void ArtifactNotFoundException_Should_Be_BinarySerializable()
        {
            var artifactNotFoundException = new ArtifactNotFoundException();

            artifactNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ArtifactNotFoundException_With_Message_Should_Be_BinarySerializable()
        {
            var artifactNotFoundException = new ArtifactNotFoundException("message");

            artifactNotFoundException.Should().BeBinarySerializable();
        }
    }
}