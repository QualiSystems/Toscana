using System;
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

        [Test]
        public void ToscaImportFileNotFoundException_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaImportFileNotFoundException = new ToscaImportFileNotFoundException("message", innerException);

            toscaImportFileNotFoundException.InnerException.Message.Should().Be("inner");
            toscaImportFileNotFoundException.Message.Should().Be("message");
        }
    }
}