using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaRelationshipTypeNotFoundExceptionTests
    {
        [Test]
        public void ToscaRelationshipTypeNotFound__With_Message_Should_Be_Serializable()
        {
            // Act
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFoundException("message");

            // Assert
            toscaRelationshipTypeNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaRelationshipTypeNotFound_Should_Be_Serializable()
        {
            // Act
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFoundException();

            // Assert
            toscaRelationshipTypeNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaRelationshipTypeNotFound_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFoundException("message", innerException);

            toscaRelationshipTypeNotFound.InnerException.Message.Should().Be("inner");
            toscaRelationshipTypeNotFound.Message.Should().Be("message");
        }
    }
}