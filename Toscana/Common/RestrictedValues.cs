using System;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RestrictedValues : ValidationAttribute
    {
        private readonly string[] validValues;

        public RestrictedValues(string[] validValues)
        {
            this.validValues = validValues;
        }

        public RestrictedValues(string[] validValues, string errorMessage) : base(errorMessage)
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