using System;
using System.Collections;
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
            var validationResults = new List<ValidationResult>();
            if (!TryValidateObjectRecursive(toscaObject, validationResults))
            {
                var message = string.Join(Environment.NewLine, validationResults.Select(r=>r.ErrorMessage).ToList());

                throw new ToscaValidationException(message);
            }
        }

        private static bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results)
        {
            bool result = TryValidateObject(obj, results);

            if (obj is string) return result;
            
            var properties = obj.GetType().GetProperties()
                .Where(prop => prop.CanRead && prop.GetIndexParameters().Length == 0)
                .ToList();

            foreach (var property in properties)
            {
                var value = obj.GetPropertyValue(property.Name);

                if (value == null) continue;

                var asEnumerable = value as IEnumerable;
                if (asEnumerable != null && !(value is string))
                {
                    foreach (var enumObj in asEnumerable)
                    {
                        result = TryValidateObjectRecursive(enumObj, results) && result;
                    }
                }
                else
                {
                    result = TryValidateObjectRecursive(value, results) && result;
                }
            }

            return result;
        }

        private static bool TryValidateObject(object obj, ICollection<ValidationResult> results)
        {
            return Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), results, true);
        }
    }
}