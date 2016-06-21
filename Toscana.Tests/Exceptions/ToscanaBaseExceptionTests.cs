using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscanaBaseExceptionTests
    {
        [Test]
        public void ToscanaBaseException_Should_Be_Binary_Serializable()
        {
            var toscanaBaseException = new ToscanaBaseException();

            toscanaBaseException.Should().BeBinarySerializable();
        }
    }
}