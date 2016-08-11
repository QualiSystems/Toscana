namespace Toscana
{
    public class ToscaObject
    {
        protected ToscaCloudServiceArchive cloudServiceArchive;
        public string DerivedFrom { get; set; }

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
    }
}