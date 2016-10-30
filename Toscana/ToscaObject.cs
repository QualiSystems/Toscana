using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsValidator;
using YamlDotNet.Serialization;
using static System.Linq.Enumerable;

namespace Toscana
{
    /// <summary>
    /// Defines TOSCA entity that supports inheritance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDerivableToscaEntity<T> where T : IDerivableToscaEntity<T>
    {
        /// <summary>
        /// An optional name of parent entity this new entity derives from.
        /// </summary>
        string DerivedFrom { get; set; }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        T GetDerivedFromEntity();

        /// <summary>
        /// Returns True if this entity is the root, which other entities derive from it.
        /// False otherwise
        /// </summary>
        /// <returns></returns>
        bool IsRoot();

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        void SetDerivedFromToRoot(string name);
    }

    /// <summary>
    /// GetDerivedFromEntity object for TOSCA entities that support inheritance
    /// </summary>
    public abstract class ToscaObject<T> : IDerivableToscaEntity<T> where T: ToscaObject<T>
    {
        /// <summary>
        /// Reference to the <see cref="ToscaCloudServiceArchive"/> the entity belongs 
        /// </summary>
        private ToscaCloudServiceArchive cloudServiceArchive;

        /// <summary>
        /// An optional name of parent entity this new entity derives from.
        /// </summary>
        public string DerivedFrom { get; set; }

        /// <summary>
        /// Reference to the <see cref="ToscaCloudServiceArchive"/> the entity belongs 
        /// </summary>
        protected ToscaCloudServiceArchive GetCloudServiceArchive()
        {
            return cloudServiceArchive;
        }

        /// <summary>
        /// Sets archive that the node belongs to
        /// </summary>
        /// <param name="newCloudServiceArchive"></param>
        internal void SetCloudServiceArchive(ToscaCloudServiceArchive newCloudServiceArchive)
        {
            cloudServiceArchive = newCloudServiceArchive;
        }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        public abstract T GetDerivedFromEntity();

        /// <summary>
        /// Returns True if this entity is the root, which other entities derive from it.
        /// False otherwise
        /// </summary>
        /// <returns></returns>
        public bool IsRoot()
        {
            return string.IsNullOrEmpty(DerivedFrom);
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public abstract void SetDerivedFromToRoot(string name);

        /// <summary>
        /// Validates for circular dependency
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ValidationResult> ValidateCircularDependency() 
        {
            List<string> inheritanceList = new List<string>();
            for (var currEntity = this; currEntity != null; currEntity = currEntity.GetDerivedFromEntity())
            {
                if (inheritanceList.Contains(currEntity.DerivedFrom))
                {
                    return new [] {new ValidationResult(string.Format("Circular dependency detected on {0}: '{1}'", GetEntityName(), currEntity.DerivedFrom), new[] { "DerivedFrom" })};
                }
                inheritanceList.Add(currEntity.DerivedFrom);
            }
            return Empty<ValidationResult>();
        }

        private string GetEntityName()
        {
            return GetType().Name.Substring(5);
        }
    }
}