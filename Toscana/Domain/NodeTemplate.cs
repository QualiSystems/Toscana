using System.Collections.Generic;

// ReSharper disable All

namespace Toscana.Domain
{
    public class NodeTemplate
    {
        public string Type { get; set; }
        public ComputeCapabilities Capabilities { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<Dictionary<string, object>> Requirements { get; set; }
        public Dictionary<string, Dictionary<string, string>> Interfaces { get; set; }
    }

    public class ComputeCapabilities
    {
        public Os Os { get; set; }
        public Host Host { get; set; }
    }
}