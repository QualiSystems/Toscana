using System;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace Toscana
{
    /// <summary>
    /// Interface to represent TOSCA.meta file content
    /// </summary>
    public interface IToscaMetadata
    {
        /// <summary>
        /// Specifies TOSCA.meta file version
        /// </summary>
        Version ToscaMetaFileVersion { get; }

        /// <summary>
        /// Denotes the verison of CSAR
        /// Due to the simplified structure of the CSAR file and TOSCA.meta file compared to TOSCA 1.0, 
        /// the CSAR-Version keyword listed in block_0 of the meta-file is required to denote version 1.1.
        /// </summary>
        Version CsarVersion { get; }

        /// <summary>
        /// Specifies who created the CSAR  
        /// </summary>
        string CreatedBy { get; }

        /// <summary>
        /// Entry-Definitions keyword pointing to a valid TOSCA definitions YAML file that a TOSCA 
        /// orchestrator should use as entry for parsing the contents of the overall CSAR file.
        /// </summary>
        string EntryDefinitions { get; }
    }

    /// <summary>
    /// Represents the content of TOSCA.met file which provides entry information for a TOSCA orchestrator 
    /// processing the CSAR file.
    /// </summary>
    public class ToscaMetadata : IToscaMetadata
    {
        /// <summary>
        /// Specifies TOSCA.meta file version
        /// </summary>
        [YamlMember(Alias = "TOSCA-Meta-File-Version")]
        [Required(ErrorMessage = "TOSCA-Meta-File-Version is required in TOSCA.meta", AllowEmptyStrings = false)]
        public Version ToscaMetaFileVersion { get; set; }

        /// <summary>
        /// Denotes the verison of CSAR
        /// Due to the simplified structure of the CSAR file and TOSCA.meta file compared to TOSCA 1.0, 
        /// the CSAR-Version keyword listed in block_0 of the meta-file is required to denote version 1.1.
        /// </summary>
        [YamlMember(Alias = "CSAR-Version")]
        [Required(ErrorMessage = "CSAR-Version is required in TOSCA.meta", AllowEmptyStrings = false)]
        public Version CsarVersion { get; set; }

        /// <summary>
        /// Specifies who created the CSAR  
        /// </summary>
        [YamlMember(Alias = "Created-By")]
        [Required(ErrorMessage = "Created-By is required in TOSCA.meta", AllowEmptyStrings = false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Entry-Definitions keyword pointing to a valid TOSCA definitions YAML file that a TOSCA 
        /// orchestrator should use as entry for parsing the contents of the overall CSAR file.
        /// </summary>
        [YamlMember(Alias = "Entry-Definitions")]
        [Required(ErrorMessage = "Entry-Definitions is required in TOSCA.meta", AllowEmptyStrings = false)]
        public string EntryDefinitions { get; set; }
    }
}