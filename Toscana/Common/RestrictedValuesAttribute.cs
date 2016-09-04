using System;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    internal class RestrictedValuesAttribute : ValidationAttribute
    {
        private readonly string[] validValues;

        public RestrictedValuesAttribute(string[] validValues)
            : this(validValues, "Specified value is not one of the valid values: " + string.Join(", ", validValues))
        {
            this.validValues = validValues;
        }

        public RestrictedValuesAttribute(string[] validValues, string errorMessage) : 
            this(validValues, () => errorMessage)
        {
        }

        protected RestrictedValuesAttribute(string[] validValues, Func<string> errorMessageAccessor)
            : base(errorMessageAccessor)
        {
            this.validValues = validValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (((string) value).EqualsAny(validValues))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage, new[] {validationContext.MemberName});
        }
    }
}