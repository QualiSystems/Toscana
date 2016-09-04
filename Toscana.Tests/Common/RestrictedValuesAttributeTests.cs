using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;

namespace Toscana.Tests.Common
{
    [TestFixture]
    public class RestrictedValuesAttributeTests
    {
        private class ClassWithRestrictedValues
        {
            [RestrictedValues(new[] {"one"})]
            public string SomeStringProperty { get; set; }
        }

        [Test]
        public void Validation_Exception_Should_Be_Thrown_With_Default_Error_Message_When_Not_Specified()
        {
            var instance = new ClassWithRestrictedValues {SomeStringProperty = "two"};
            Action action = () => Validator.ValidateObject(instance, new ValidationContext(instance), true);

            action.ShouldThrow<ValidationException>().WithMessage("Specified value is not one of the valid values: one");
        }
    }
}