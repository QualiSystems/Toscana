using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaImportFileNotFoundExceptionTests
    {
        [Test]
        public void ToscaImportFileNotFoundException_Should_BeBinarySerializable()
        {
            var toscaImportFileNotFoundException = new ToscaImportFileNotFoundException();

            toscaImportFileNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaImportFileNotFoundException_With_Message_Should_BeBinarySerializable()
        {
            var toscaImportFileNotFoundException = new ToscaImportFileNotFoundException("message");

            toscaImportFileNotFoundException.Should().BeBinarySerializable();
        }
    }
}