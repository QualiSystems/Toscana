using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscanaValidationExceptionTests
    {
        [Test]
        public void ToscanaValidationException_Should_Be_Binary_Serializable()
        {
            var toscanaValidationException = new ToscanaValidationException();

            toscanaValidationException.Should().BeBinarySerializable();
        }
    }
}