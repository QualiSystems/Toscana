﻿using System.Collections.Generic;
using System.IO;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaCloudServiceArchive
    {
        public const string ToscaMetaFileName = "TOSCA.meta";

        public ToscaCloudServiceArchive()
        {
            ToscaSimpleProfiles = new Dictionary<string, ToscaSimpleProfile>();
            ToscaMetadata = new ToscaMetadata();
        }

        public Dictionary<string, ToscaSimpleProfile> ToscaSimpleProfiles { get; set; }
        public ToscaMetadata ToscaMetadata { get; set; }

        public static ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null)
        {
            var toscaCloudServiceArchiveLoader = GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveFilePath, alternativePath);
        }

        public static ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null)
        {
            var toscaCloudServiceArchiveLoader = GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveStream, alternativePath);
        }

        private static IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return new Bootstrapper().GetToscaCloudServiceArchiveLoader();
        }
    }
}