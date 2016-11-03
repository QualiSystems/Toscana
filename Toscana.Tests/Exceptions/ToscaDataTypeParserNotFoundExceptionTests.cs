using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests.Exceptions
{
    [TestFixture]
    public class ToscaDataTypeParserNotFoundExceptionTests
    {
        [Test]
        public void I_Can_Serialize_ToscaDataTypeParserNotFoundException()
        {
            //TestCop		
            var toscaDataTypeParserNotFoundException = new ToscaDataTypeParserNotFoundException();

            toscaDataTypeParserNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void I_Can_Serialize_ToscaDataTypeParserNotFoundException_With_Message()
        {
            //TestCop		
            var toscaDataTypeParserNotFoundException = new ToscaDataTypeParserNotFoundException("message");

            toscaDataTypeParserNotFoundException.Should().BeBinarySerializable();
        }

        [Test]
        public void ToscaDataTypeParserNotFoundException_With_Message_And_Inner_Exception_Properly_Initialized()
        {
            //TestCop		
            var toscaDataTypeParserNotFoundException = new ToscaDataTypeParserNotFoundException("message", new Exception("inner"));

            // Assert
            toscaDataTypeParserNotFoundException.InnerException.Message.Should().Be("inner");
            toscaDataTypeParserNotFoundException.Message.Should().Be("message");
        }
    }
}