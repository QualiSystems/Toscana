using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class Artifact
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string File { get; set; }
        public string Repository { get; set; }
        public string Description { get; set; }
        public string DeployPath { get; set; }
    }
}