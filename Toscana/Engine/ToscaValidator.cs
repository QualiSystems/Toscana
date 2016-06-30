using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaValidator<in T>
    {
        void Validate(T toscaObject);
    }

    public class ToscaValidator<T> : IToscaValidator<T>
    {
        public void Validate(T toscaObject)
        {
            if (toscaObject == null)
            {
                throw new ToscaValidationException("Tosca is null or empty");
            }

            var dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();
            
            var validationResults = new List<ValidationResult>();
            if (!dataAnnotationsValidator.TryValidateObjectRecursive<T>(toscaObject, validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }
    }
}