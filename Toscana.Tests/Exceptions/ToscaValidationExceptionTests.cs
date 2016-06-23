using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaValidationExceptionTests
    {
        [Test]
        public void ToscanaValidationException_Should_Be_Binary_Serializable()
        {
            var toscanaValidationException = new ToscaValidationException();

            toscanaValidationException.Should().BeBinarySerializable();
        }
    }
}