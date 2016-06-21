using System;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RestrictedValues : ValidationAttribute
    {
        private readonly string[] validValues;

        public RestrictedValues(string[] validValues)
            : this(validValues, "Specified value is not one of the valid values: " + string.Join(", ", validValues))
        {
            this.validValues = validValues;
        }

        public RestrictedValues(string[] validValues, string errorMessage) : 
            this(validValues, () => errorMessage)
        {
        }

        protected RestrictedValues(string[] validValues, Func<string> errorMessageAccessor)
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