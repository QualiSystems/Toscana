using System.Collections.Generic;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// A Relationship Type is a reusable entity that defines the type of one or more relationships 
    /// between Node Types or Node Templates.
    /// </summary>
    public class ToscaRelationshipType : ToscaObject<ToscaRelationshipType>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaRelationshipType"/>
        /// </summary>
        public ToscaRelationshipType()
        {
            Properties = new List<ToscaPropertyDefinition>();
            Attributes = new List<ToscaAttributeDefinition>();
            Interfaces = new Dictionary<string, ToscaInterfaceDefinition>();
            ValidTargetTypes = new List<string>();
        }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        /// <exception cref="ToscaRelationshipTypeNotFound" accessor="get">Thrown when relationship type not found in any of the service templates.</exception>
        public override ToscaRelationshipType Base
        {
            get
            {
                if (CloudServiceArchive == null || IsRoot()) return null;
                ToscaRelationshipType relationshipType;
                if (CloudServiceArchive.RelationshipTypes.TryGetValue(DerivedFrom, out relationshipType))
                {
                    return relationshipType;
                }
                throw new ToscaRelationshipTypeNotFound(string.Format("Relationship type '{0}' not found", DerivedFrom));
            }
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public override void SetDerivedFromToRoot(string name)
        {
            if (name != ToscaDefaults.ToscaRelationshipTypeRoot && IsRoot())
            {
                DerivedFrom = ToscaDefaults.ToscaRelationshipTypeRoot;
            }
        }

        /// <summary>
        /// An optional version for the Relationship Type definition.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// An optional description for the Relationship Type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property definitions for the Relationship Type.
        /// </summary>
        public List<ToscaPropertyDefinition> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Relationship Type.
        /// </summary>
        public List<ToscaAttributeDefinition> Attributes { get; set; }

        /// <summary>
        /// An optional list of interface definitions interfaces supported by the Relationship Type.
        /// </summary>
        public Dictionary<string, ToscaInterfaceDefinition> Interfaces { get; set; }

        /// <summary>
        /// An optional list of one or more names of Capability Types that are valid targets for this relationship.
        /// </summary>
        public List<string> ValidTargetTypes { get; set; }
    }
}