using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    public class ToscaImport
    {
        [Required(ErrorMessage = "file is required on import")]
        public string File { get; set; }

        public string Repository { get; set; }

        public string NamespaceUri { get; set; }

        public string NamespacePrefix { get; set; }

        public static implicit operator ToscaImport(string val)
        {
            return new ToscaImport { File = val};
        }
    }
}