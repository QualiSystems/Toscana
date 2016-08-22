using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// The Credential type is a complex TOSCA data Type used when describing authorization 
    /// credentials used to access network accessible resources.
    /// </summary>
    public class ToscaCredential
    {
        /// <summary>
        /// The optional protocol name.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// The required token type.
        /// </summary>
        [Required(ErrorMessage = "TokenType is required on credential", AllowEmptyStrings = false)]
        public string TokenType { get; set; }

        /// <summary>
        /// The required token used as a credential for authorization or access to a networked resource.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The optional list of protocol-specific keys or assertions.
        /// </summary>
        public Dictionary<string, string> Keys { get; set; }

        /// <summary>
        /// The optional user (name or ID) used for non-token based credentials.
        /// </summary>
        public string User { get; set; }
    }
}