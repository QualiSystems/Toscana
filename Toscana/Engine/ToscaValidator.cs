using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaValidator
    {
        void Validate(ToscaSimpleProfile toscaSimpleProfile);
    }

    public class ToscaValidator : IToscaValidator
    {
        public void Validate(ToscaSimpleProfile toscaSimpleProfile)
        {
            var dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();
            
            var validationResults = new List<ValidationResult>();
            if (!dataAnnotationsValidator.TryValidateObjectRecursive(toscaSimpleProfile, validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }
    }
}