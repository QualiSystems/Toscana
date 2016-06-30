using System;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace Toscana
{
    public class ToscaMetadata
    {
        [YamlMember(Alias = "TOSCA-Meta-File-Version")]
        [Required(ErrorMessage = "TOSCA-Meta-File-Version is required in TOSCA.meta", AllowEmptyStrings = false)]
        public Version ToscaMetaFileVersion { get; set; }

        [YamlMember(Alias = "CSAR-Version")]
        [Required(ErrorMessage = "CSAR-Version is required in TOSCA.meta", AllowEmptyStrings = false)]
        public Version CsarVersion { get; set; }

        [YamlMember(Alias = "Created-By")]
        [Required(ErrorMessage = "Created-By is required in TOSCA.meta", AllowEmptyStrings = false)]
        public string CreatedBy { get; set; }

        [YamlMember(Alias = "Entry-Definitions")]
        [Required(ErrorMessage = "Entry-Definitions is required in TOSCA.meta", AllowEmptyStrings = false)]
        public string EntryDefinitions { get; set; }
    }
}