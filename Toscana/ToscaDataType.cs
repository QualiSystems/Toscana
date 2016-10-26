using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// A Data Type definition defines the schema for new named datatypes in TOSCA. 
    /// </summary>
    public class ToscaDataType : ToscaObject<ToscaDataType>, IValidatableObject
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaDataType"/>
        /// </summary>
        public ToscaDataType()
        {
            Properties = new Dictionary<string, ToscaProperty>();
        }

        /// <summary>
        /// An optional version for the Data Type definition.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// The optional description for the Data Type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The optional list of sequenced constraint clauses for the Data Type. 
        /// </summary>
        public List<Dictionary<string, object>> Constraints { get; set; }

        /// <summary>
        /// The optional list property definitions that comprise the schema for a complex Data Type in TOSCA.
        /// </summary>
        public Dictionary<string, ToscaProperty> Properties { get; set; }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        /// <exception cref="ToscaDataTypeNotFoundException" accessor="get">When derived from data type not found in the Cloud Service Archive.</exception>
        public override ToscaDataType GetDerivedFromEntity()
        {
            if (GetCloudServiceArchive() == null || IsRoot()) return null;
            ToscaDataType datatype;
            if (GetCloudServiceArchive().DataTypes.TryGetValue(DerivedFrom, out datatype))
            {
                return datatype;
            }
            throw new ToscaDataTypeNotFoundException(string.Format("Data type '{0}' not found", DerivedFrom));
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public override void SetDerivedFromToRoot(string name)
        {
            if (name != ToscaDefaults.ToscaDataTypeRoot && IsRoot())
            {
                DerivedFrom = ToscaDefaults.ToscaDataTypeRoot;
            }
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            return ValidateCircularDependency();
        }
    }
}