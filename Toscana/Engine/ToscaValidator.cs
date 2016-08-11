using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaValidator<in T>
    {
        void Validate(T toscaObject);
        bool TryValidateRecursively(T toscaObject, out List<ValidationResult> validationResults);
    }

    public class ToscaValidator<T> : IToscaValidator<T>
    {
        public void Validate(T toscaObject)
        {
            if (toscaObject == null)
            {
                throw new ToscaValidationException("Tosca is null or empty");
            }

            List<ValidationResult> validationResults;
            if (!TryValidateRecursively(toscaObject, out validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }

        public bool TryValidateRecursively(T toscaObject, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();

            try
            {
                dataAnnotationsValidator.TryValidateObjectRecursive(toscaObject, validationResults);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                validationResults.Add(new ValidationResult(targetInvocationException.InnerException.Message));
            }
            return !validationResults.Any();
        }
    }
}