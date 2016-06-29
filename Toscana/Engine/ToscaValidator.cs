using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaValidator
    {
        void Validate(ToscaServiceTemplate toscaServiceTemplate);
    }

    public class ToscaValidator : IToscaValidator
    {
        public void Validate(ToscaServiceTemplate toscaServiceTemplate)
        {
            if (toscaServiceTemplate == null)
            {
                throw new ToscaValidationException("Tosca is null or empty");
            }

            var dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();
            
            var validationResults = new List<ValidationResult>();
            if (!dataAnnotationsValidator.TryValidateObjectRecursive(toscaServiceTemplate, validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }
    }
}