

using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaServiceTemplateAlreadyExistsExceptionTests
    {
        [Test]
        public void ToscaServiceTemplateAlreadyExistsException_Should_Be_Serializable()
        {
            var toscaServiceTemplateAlreadyExistsException = new ToscaServiceTemplateAlreadyExistsException();

            toscaServiceTemplateAlreadyExistsException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaServiceTemplateAlreadyExistsException_With_Message_Should_Be_Serializable()
        {
            var toscaServiceTemplateAlreadyExistsException = new ToscaServiceTemplateAlreadyExistsException("message");

            toscaServiceTemplateAlreadyExistsException.Should().BeBinarySerializable();
        }
    }
}

