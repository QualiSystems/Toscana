using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A repository definition defines a named external repository which contains deployment 
    /// and implementation artifacts that are referenced within the TOSCA Service Template.
    /// </summary>
    public class ToscaRepository
    {
        /// <summary>
        /// The optional description for the repository.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The required URL or network address used to access the repository.
        /// </summary>
        [Required(ErrorMessage = "Url is required on repository", AllowEmptyStrings = false)]
        public string Url { get; set; }

        /// <summary>
        /// The optional Credential used to authorize access to the repository.
        /// </summary>
        public ToscaCredential Credential { get; set; }
    }
}