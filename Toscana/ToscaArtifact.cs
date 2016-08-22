using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// An artifact definition defines a named, typed file that can be associated with Node Type or Node Template 
    /// and used by orchestration engine to facilitate deployment and implementation of interface operations.
    /// </summary>
    public class ToscaArtifact
    {
        /// <summary>
        /// The required artifact type for the artifact definition.
        /// </summary>
        [Required(ErrorMessage = "type is required on artifact")]
        public string Type { get; set; }

        /// <summary>
        /// The required URI string (relative or absolute) which can be used to locate the artifact’s file.
        /// </summary>
        [Required(ErrorMessage = "file is required on artifact")]
        public string File { get; set; }

        /// <summary>
        /// The optional name of the repository definition which contains the location of the external repository that contains the artifact.  
        /// The artifact is expected to be referenceable by its file URI within the repository.
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// The optional description for the artifact definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The file path the associated file would be deployed into within the target node’s container.
        /// </summary>
        public string DeployPath { get; set; }
    }
}