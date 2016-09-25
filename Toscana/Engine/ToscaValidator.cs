using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    internal interface IToscaValidator<in T>
    {
        void Validate(T toscaObject);
        bool TryValidateRecursively(T toscaObject, out List<ValidationResult> validationResults);
    }

    internal class ToscaValidator<T> : IToscaValidator<T>
    {
        /// <summary>
        /// Validates a TOSCA entity and throws exception if validation fails
        /// </summary>
        /// <param name="toscaObject"></param>
        /// <exception cref="ToscaValidationException">Tosca entity is null or not valid</exception>
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
                validationResults = validationResults.Distinct(new ValidationResultEqualityComparer()).ToList();
            }
            catch (TargetInvocationException targetInvocationException)
            {
                validationResults.Add(new ValidationResult(targetInvocationException.InnerException.Message));
            }
            return !validationResults.Any();
        }
    }
}