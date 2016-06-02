using System.Collections.Generic;

namespace Toscana.Domain
{
    public class TopologyInput
    {
        public TopologyInput()
        {
            Constraints = new List<Dictionary<string, List<int>>>();
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public List<Dictionary<string,List<int>>> Constraints { get; set; }
    }
}