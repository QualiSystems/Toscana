using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// The optional key that is used to declare the name of the Datatype definition for entries of set types such as the TOSCA list or map.
    /// </summary>
    public class ToscaPropertyEntrySchema
    {
        /// <summary>
        ///  Represents the optional description of the entry schema.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the required type name for entries in a list or map  property type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Represents the optional sequenced list of one or more constraint clauses on entries in a list or map property type.
        /// </summary>
        public List<Dictionary<string, object>> Constraints { get; set; }

        /// <summary>
        /// Initializes <see cref="ToscaPropertyEntrySchema"/> by a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static implicit operator ToscaPropertyEntrySchema(string type)
        {
            return new ToscaPropertyEntrySchema() { Type = type };
        }
    }
}