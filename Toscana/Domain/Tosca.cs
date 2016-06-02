using YamlDotNet.Serialization;

namespace Toscana.Domain
{
    public class Tosca
    {
        [YamlAlias("tosca_definitions_version")]
        public string ToscaDefinitionsVersion { get; set; }
        public string Description { get; set; }
        [YamlAlias("topology_template")]
        public TopologyTemplate TopologyTemplate { get; set; }
    }
}