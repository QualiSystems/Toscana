using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Toscana.Domain
{
    public class TopologyTemplate
    {
        [YamlAlias("node_templates")]
        public Dictionary<string, NodeTemplate> NodeTemplates { get; set; }

        public Dictionary<string, TopologyInput> Inputs { get; set; }

        public Dictionary<string, TopologyOutput> Outputs { get; set; }
    }
}