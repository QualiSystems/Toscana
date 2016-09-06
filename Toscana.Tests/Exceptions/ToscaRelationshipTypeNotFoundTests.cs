using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaRelationshipTypeNotFoundTests
    {
        [Test]
        public void ToscaRelationshipTypeNotFound__With_Message_Should_Be_Serializable()
        {
            // Act
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFound("message");

            // Assert
            toscaRelationshipTypeNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaRelationshipTypeNotFound_Should_Be_Serializable()
        {
            // Act
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFound();

            // Assert
            toscaRelationshipTypeNotFound.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaRelationshipTypeNotFound_With_Inner_Exception_Initialized_Properly()
        {
            var innerException = new Exception("inner");
            var toscaRelationshipTypeNotFound = new ToscaRelationshipTypeNotFound("message", innerException);

            toscaRelationshipTypeNotFound.InnerException.Message.Should().Be("inner");
            toscaRelationshipTypeNotFound.Message.Should().Be("message");
        }
    }
}