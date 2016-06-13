using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class ToscaImport
    {
        [Required(ErrorMessage = "file is required on import")]
        public string File { get; set; }

        public string Repository { get; set; }

        public string NamespaceUri { get; set; }

        public string NamespacePrefix { get; set; }
    }
}