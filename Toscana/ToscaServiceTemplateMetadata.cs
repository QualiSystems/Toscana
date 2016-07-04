using System;
using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// Represents metadata node in TOSCA Service Template YAML file
    /// </summary>
    public class ToscaServiceTemplateMetadata : Dictionary<string, object>
    {
        private const string TemplateAuthorKeyName = "template_author";
        private const string TemplateNameKeyName = "template_name";
        private const string TemplateVersionKeyName = "template_version";

        /// <summary>
        /// Gets or sets TOSCA Service Template name
        /// </summary>
        public string TemplateName
        {
            get { return SafelyGetValue(TemplateNameKeyName); }
            set { this[TemplateNameKeyName] = value; }
        }

        /// <summary>
        /// Gets or sets TOSCA Service Template author
        /// </summary>
        public string TemplateAuthor
        {
            get
            {
                return SafelyGetValue(TemplateAuthorKeyName);
            }
            set { this[TemplateAuthorKeyName] = value; }
        }

        /// <summary>
        /// Gets or sets TOSCA Service Template version
        /// </summary>
        public Version TemplateVersion
        {
            get
            {
                var versionAsString = SafelyGetValue(TemplateVersionKeyName);
                if (string.IsNullOrEmpty(versionAsString)) return null;
                return new Version(versionAsString);
            }
            set { this[TemplateVersionKeyName] = value.ToString(); }
        }

        private string SafelyGetValue(string keyWord)
        {
            if (ContainsKey(keyWord))
            {
                return this[keyWord].ToString();
            }
            return string.Empty;
        }
    }
}