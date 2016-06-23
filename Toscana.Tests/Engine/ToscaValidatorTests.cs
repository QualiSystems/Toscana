using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaValidatorTests
    {
        [Test]
        public void Validation_Exception_Should_Be_Thrown_When_Null_Validated()
        {
            var toscaValidator = new ToscaValidator();

            Action action = () => toscaValidator.Validate(null);

            action.ShouldThrow<ToscaValidationException>().WithMessage("Tosca is null or empty");
        }
    }
}