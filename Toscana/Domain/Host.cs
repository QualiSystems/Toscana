using YamlDotNet.Serialization;

namespace Toscana.Domain
{
    public class Host : Capability
    {
        public HostProperties Properties { get; set; }
    }

    public class HostProperties
    {
        public object NumCpus { get; set; }

        public DigitalStorage DiskSize { get; set; }

        public DigitalStorage MemSize { get; set; }
    }
}