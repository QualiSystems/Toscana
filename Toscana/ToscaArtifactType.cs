using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// An Artifact Type is a reusable entity that defines the type of one or more files that are used to define implementation or deployment artifacts that are referenced by nodes or relationships on their operations
    /// </summary>
    public class ToscaArtifactType : ToscaObject<ToscaArtifactType>, IValidatableObject
    {
        /// <summary>
        /// Initilizes an instance of <see cref="ToscaArtifactType"/>
        /// </summary>
        public ToscaArtifactType()
        {
            string[] a = new string[0];
            Properties = new Dictionary<string, ToscaProperty>();
        }

        /// <summary>
        /// An optional version for the Artifact Type definition.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// An optional description for the Artifact Type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The required mime type property for the Artifact Type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// The required file extension property for the Artifact Type.
        /// </summary>
        public string[] FileExt { get; set; }

        /// <summary>
        /// An optional list of property definitions for the Artifact Type.
        /// </summary>
        public Dictionary<string, ToscaProperty> Properties { get; set; }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        public override ToscaArtifactType GetDerivedFromEntity()
        {
            if (CloudServiceArchive == null || IsRoot()) return null;
            ToscaArtifactType artifactType;
            if (CloudServiceArchive.ArtifactTypes.TryGetValue(DerivedFrom, out artifactType))
            {
                return artifactType;
            }
            throw new ToscaNodeTypeNotFoundException(string.Format("Artifact type '{0}' not found", DerivedFrom));
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public override void SetDerivedFromToRoot(string name)
        {
            if (name != ToscaDefaults.ToscaArtifactRoot && IsRoot())
            {
                DerivedFrom = ToscaDefaults.ToscaArtifactRoot;
            }
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            return ValidateCircularDependency();
        }
    }
}