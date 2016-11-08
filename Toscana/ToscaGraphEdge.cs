using QuickGraph;

namespace Toscana
{
    internal class ToscaGraphEdge : IEdge<string>
    {
        private readonly string source;
        private readonly string target;

        public ToscaGraphEdge(string source, string target)
        {
            this.source = source;
            this.target = target;
        }

        public string Source
        {
            get { return source; }
        }

        public string Target
        {
            get { return target; }
        }
    }
}