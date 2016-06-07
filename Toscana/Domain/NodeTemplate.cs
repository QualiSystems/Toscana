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
        public Dictionary<string, object> Interfaces { get; set; }
        public Dictionary<string, Artifact> Artifacts { get; set; }
    }

    public class Artifact
    {
        public string Type { get; set; }
        public string File { get; set; }
    }

    public class ComputeCapabilities
    {
        public Os Os { get; set; }
        public Host Host { get; set; }
    }
}