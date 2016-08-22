using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Engine
{
    internal class ValidationResultEqualityComparer : IEqualityComparer<ValidationResult>
    {
        public bool Equals(ValidationResult x, ValidationResult y)
        {
            return string.Compare(x.ErrorMessage, y.ErrorMessage, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public int GetHashCode(ValidationResult obj)
        {
            return obj.ErrorMessage.GetHashCode();
        }
    }
}