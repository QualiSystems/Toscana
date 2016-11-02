using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Engine;
using Toscana.Exceptions;
using YamlDotNet.Serialization;

namespace Toscana
{
    /// <summary>
    ///     A property definition defines a named, typed value and related data that
    ///     can be associated with an entity defined in this specification (e.g., Node Types,
    ///     Relationship Types, Capability Types, etc.).
    ///     Properties are used by template authors to provide input values to TOSCA entities which
    ///     indicate their “desired state” when they are instantiated.  The value of a property can be
    ///     retrieved using the get_property function within TOSCA Service Templates.
    /// </summary>
    public class ToscaProperty : IValidatableObject
    {
        private const string ValidValues = "valid_values";

        /// <summary>
        ///     Initializes an instance of ToscaProperty
        /// </summary>
        public ToscaProperty()
        {
            Required = true;
            Constraints = new List<Dictionary<string, object>>();
        }

        /// <summary>
        ///     The required data type for the property.
        /// </summary>
        [Required(ErrorMessage = "type is required on property", AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        ///     The optional description for the property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     An optional key that declares a property as required (true) or not (false).
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        ///     An optional key that may provide a value to be used as a default if not provided by another means.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        ///     The optional status of the property relative to the specification or implementation.
        /// </summary>
        public ToscaPropertyStatus Status { get; set; }

        /// <summary>
        ///     The optional list of sequenced constraint clauses for the property.
        /// </summary>
        public List<Dictionary<string, object>> Constraints { get; set; }

        /// <summary>
        ///     The optional key that is used to declare the name of the Datatype definition for entries of set types such as the
        ///     TOSCA list or map.
        /// </summary>
        public ToscaPropertyEntrySchema EntrySchema { get; set; }

        /// <summary>
        ///     The optional list of tags
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        ///     String representation of Default property.
        ///     Returns empty string when default is null
        /// </summary>
        [YamlIgnore]
        public string StringValue
        {
            get { return Default == null ? String.Empty : Default.ToString(); }
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            var validValuesConstraint = Constraints.FirstOrDefault(c => c.ContainsKey(ValidValues));
            if (validValuesConstraint != null)
            {
                var parser = DependencyResolver.GetParser(Type);
                if (parser == null)
                {
                    throw new ToscaDataTypeParserNotFoundException(string.Format("Parser for data type '{0}' nod found.", Type));
                }
                var validValues = validValuesConstraint[ValidValues];
                if (!(validValues is List<object>))
                {
                    return Enumerable.Empty<ValidationResult>();
                }

                var listOfValidValues = validValues as List<object>;
                foreach (var validValue in listOfValidValues)
                {
                    object result;
                    if (!parser.TryParse(validValue, out result))
                    {
                        validationResults.Add(
                            new ValidationResult(
                                String.Format(
                                    "Value '{0}' of constraint '{1}' cannot be parsed according to property data type '{2}'",
                                    validValue, String.Join(",", listOfValidValues), Type)));
                    }
                }
            }
            return validationResults;
        }

        /// <summary>
        /// Adds a constraint to the property definition
        /// </summary>
        /// <param name="contraintName">Constraint name, like greater_or_equal, valid_values</param>
        /// <param name="constraintValue">Constraint values</param>
        public void AddConstraint(string contraintName, object constraintValue)
        {
            Constraints.Add(new Dictionary<string, object> { {contraintName, constraintValue} });
        }

        /// <summary>
        /// Returns contstrains as a dictionary
        /// </summary>
        /// <returns>Contstrains as a dictionary</returns>
        public Dictionary<string, object> GetConstraintsDictionary()
        {
            return Constraints.SelectMany(a=>a).ToDictionary(c=>c.Key, d=>d.Value);
        }

        /// <summary>
        /// Sets constraints of the property
        /// </summary>
        /// <param name="constraints">Constraints to set</param>
        public void SetConstraints(Dictionary<string, object> constraints)
        {
            Constraints = constraints.Select(c => new Dictionary<string, object> {{c.Key, c.Value}}).ToList();
        }

        /// <summary>
        /// Clones the property 
        /// </summary>
        /// <returns>A new instance of property with same attribute values</returns>
        public ToscaProperty Clone()
        {
            return (ToscaProperty) MemberwiseClone();
        }
    }
}