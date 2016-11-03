using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaServiceTemplateAlreadyExistsExceptionTests
    {
        [Test]
        public void ToscaServiceTemplateAlreadyExistsException_Initialized_With_Message_And_Inner_Exception()
        {
            // Act
            var toscaServiceTemplateAlreadyExistsException = new ToscaServiceTemplateAlreadyExistsException("message",
                new Exception("inner"));

            // Assert
            toscaServiceTemplateAlreadyExistsException.InnerException.Message.Should().Be("inner");
            toscaServiceTemplateAlreadyExistsException.Message.Should().Be("message");
        }

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