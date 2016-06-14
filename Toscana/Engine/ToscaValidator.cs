using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Domain;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaValidator
    {
        void Validate(Tosca toscaObject);
    }

    public class ToscaValidator : IToscaValidator
    {
        public void Validate(Tosca toscaObject)
        {
            var dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();
            
            var validationResults = new List<ValidationResult>();
            if (!dataAnnotationsValidator.TryValidateObjectRecursive(toscaObject, validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }
    }
}