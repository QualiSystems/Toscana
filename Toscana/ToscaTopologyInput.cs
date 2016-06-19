using System.Collections.Generic;

namespace Toscana
{
    public class ToscaTopologyInput
    {
        public ToscaTopologyInput()
        {
            Constraints = new List<Dictionary<string, List<int>>>();
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public List<Dictionary<string,List<int>>> Constraints { get; set; }
    }
}