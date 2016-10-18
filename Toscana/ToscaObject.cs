using YamlDotNet.Serialization;

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
        T Base { get; }

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
    /// Base object for TOSCA entities that support inheritance
    /// </summary>
    public abstract class ToscaObject<T> : IDerivableToscaEntity<T> where T: ToscaObject<T>
    {
        /// <summary>
        /// Reference to the <see cref="ToscaCloudServiceArchive"/> the entity belongs 
        /// </summary>
        protected ToscaCloudServiceArchive CloudServiceArchive;

        /// <summary>
        /// An optional name of parent entity this new entity derives from.
        /// </summary>
        public string DerivedFrom { get; set; }

        /// <summary>
        /// Returns an entity that this entity derives from.
        /// If this entity is root, null will be returned
        /// If this entity derives from a non existing entity exception will be thrown
        /// </summary>
        [YamlIgnore]
        public abstract T Base
        {
            get;
        }

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
        /// Sets archive that the node belongs to
        /// </summary>
        /// <param name="cloudServiceArchive"></param>
        internal void SetToscaCloudServiceArchive(ToscaCloudServiceArchive cloudServiceArchive)
        {
            CloudServiceArchive = cloudServiceArchive;
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public abstract void SetDerivedFromToRoot(string name);
    }
}