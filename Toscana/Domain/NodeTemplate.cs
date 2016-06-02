namespace Toscana.Domain
{
    public class NodeTemplate
    {
        public string Type { get; set; }
        public ComputeCapabilities Capabilities { get; set; }
    }

    public class ComputeCapabilities
    {
        public Os Os { get; set; }
        public Host Host { get; set; }
    }
}