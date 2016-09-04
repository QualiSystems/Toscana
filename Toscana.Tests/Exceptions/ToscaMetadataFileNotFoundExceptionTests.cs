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
    }
}