using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaNodeTypeNotFoundExceptionTests
    {
        [Test]
        public void ToscaNodeTypeNotFoundException_Should_BeBinarySerializable()
        {
            var toscaNodeTypeNotFoundException = new ToscaNodeTypeNotFoundException();

            toscaNodeTypeNotFoundException.Should().BeBinarySerializable();
        }
    }
}