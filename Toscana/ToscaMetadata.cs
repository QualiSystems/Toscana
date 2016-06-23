using System;
using YamlDotNet.Serialization;

namespace Toscana
{
    public class ToscaMetadata
    {
        [YamlMember(Alias = "TOSCA-Meta-File-Version")]
        public Version ToscaMetaFileVersion { get; set; }

        [YamlMember(Alias = "CSAR-Version")]
        public Version CsarVersion { get; set; }

        [YamlMember(Alias = "Created-By")]
        public string CreatedBy { get; set; }

        [YamlMember(Alias = "Entry-Definitions")]
        public string EntryDefinitions { get; set; }
    }
}