using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class ToscaArtifact
    {
        [Required(ErrorMessage = "type is required on artifact")]
        public string Type { get; set; }

        [Required(ErrorMessage = "file is required on artifact")]
        public string File { get; set; }

        public string Repository { get; set; }
        public string Description { get; set; }
        public string DeployPath { get; set; }
    }
}