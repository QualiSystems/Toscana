using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaMetadataFileNotFoundTests
    {
        [Test]
        public void ToscaMetadataFileNotFound_Should_BeBinarySerializable()
        {
            var toscaMetadataFileNotFound = new ToscaMetadataFileNotFound();

            toscaMetadataFileNotFound.Should().BeBinarySerializable();
        }
    }
}