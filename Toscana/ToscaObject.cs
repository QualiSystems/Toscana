namespace Toscana
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ToscaObject
    {
        protected ToscaCloudServiceArchive cloudServiceArchive;

        /// <summary>
        /// Denotes the base object it derives from
        /// </summary>
        public string DerivedFrom { get; set; }

        /// <summary>
        /// 
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