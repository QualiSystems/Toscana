using System.Collections.Generic;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaCloudServiceArchive
    {
        public ToscaCloudServiceArchive()
        {
            ToscaSimpleProfiles = new Dictionary<string, ToscaSimpleProfile>();
        }

        public Dictionary<string, ToscaSimpleProfile> ToscaSimpleProfiles { get; set; }
        public ToscaMetadata ToscaMetadata { get; set; }

        public static ToscaCloudServiceArchive Load(string archiveFilePath)
        {
            var toscaCloudServiceArchiveLoader = Bootstrapper.GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveFilePath);
        }
    }
}