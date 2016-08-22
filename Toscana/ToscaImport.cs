using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// An import definition is used within a TOSCA Service Template to locate and uniquely 
    /// name another TOSCA Service Template file which has type and template definitions 
    /// to be imported (included) and referenced within another Service Template.
    /// </summary>
    public class ToscaImport
    {
        /// <summary>
        /// The required symbolic name for the imported file.
        /// </summary>
        [Required(ErrorMessage = "file is required on import", AllowEmptyStrings = false)]
        public string File { get; set; }

        /// <summary>
        /// The optional symbolic name of the repository definition where the imported file can be found as a string.
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// The optional namespace URI to that will be applied to type definitions found within the imported file as a string
        /// </summary>
        public string NamespaceUri { get; set; }

        /// <summary>
        /// The optional namespace prefix (alias) that will be used to indicate the namespace_uri 
        /// when forming a qualified name (i.e., qname) when referencing type definitions from the imported file.
        /// </summary>
        public string NamespacePrefix { get; set; }

        /// <summary>
        /// Initializes an instance of ToscaImport and set File property
        /// </summary>
        /// <param name="val">value to set to File property</param>
        /// <returns>An instance of ToscaImport with the File value set</returns>
        public static implicit operator ToscaImport(string val)
        {
            return new ToscaImport { File = val};
        }
    }
}