using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaValidatorTests
    {
        [Test]
        public void Validation_Exception_Should_Be_Thrown_When_Null_Validated()
        {
            var toscaValidator = new ToscaValidator<ToscaServiceTemplate>();

            Action action = () => toscaValidator.Validate(null);

            action.ShouldThrow<ToscaValidationException>().WithMessage("Tosca is null or empty");
        }
    }
}