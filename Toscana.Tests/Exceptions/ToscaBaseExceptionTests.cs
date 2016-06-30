using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaBaseExceptionTests
    {
        [Test]
        public void ToscanaBaseException_Should_Be_Binary_Serializable()
        {
            var toscanaBaseException = new ToscaBaseException();

            toscanaBaseException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscanaBaseException_With_Message_Should_Be_Binary_Serializable()
        {
            var toscanaBaseException = new ToscaBaseException("message");

            toscanaBaseException.Should().BeBinarySerializable();
        }
    }
}