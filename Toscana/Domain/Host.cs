using YamlDotNet.Serialization;

namespace Toscana.Domain
{
    public class Host : Capability
    {
        public HostProperties Properties { get; set; }
    }

    public class HostProperties
    {
        [YamlAlias("num_cpus")]
        public object NumCpus { get; set; }

        [YamlAlias("disk_size")]
        public string DiskSize { get; set; }

        [YamlAlias("mem_size")]
        public string MemSize { get; set; }
    }
}