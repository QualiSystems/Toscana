namespace Toscana
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ToscaObject
    {
        protected ToscaCloudServiceArchive cloudServiceArchive;

        /// <summary>
        /// An optional name of parent entity this new entity derives from.
        /// </summary>
        public string DerivedFrom { get; set; }

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
        /// <param name="newCloudServiceArchive"></param>
        public void SetToscaCloudServiceArchive(ToscaCloudServiceArchive newCloudServiceArchive)
        {
            cloudServiceArchive = newCloudServiceArchive;
        }

        /// <summary>
        /// Sets DerivedFrom to point to root if it's not set
        /// </summary>
        /// <param name="name">Object name</param>
        public abstract void SetDerivedFromToRoot(string name);
    }
}