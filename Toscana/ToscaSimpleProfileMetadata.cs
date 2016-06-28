using System;
using System.Collections.Generic;

namespace Toscana
{
    public class ToscaSimpleProfileMetadata : Dictionary<string, object>
    {
        private const string TemplateAuthorKeyName = "template_author";
        private const string TemplateNameKeyName = "template_name";
        private const string TemplateVersionKeyName = "template_version";

        public string TemplateName
        {
            get { return SafelyGetValue(TemplateNameKeyName); }
        }

        public string TemplateAuthor
        {
            get
            {
                return SafelyGetValue(TemplateAuthorKeyName);
            }
            set { this[TemplateAuthorKeyName] = value; }
        }

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