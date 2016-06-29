using System;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace Toscana.Engine
{
    public class NodeTypeWalker
    {
        private readonly Action<string> action;
        private readonly AdjacencyGraph<string, ToscaGraphEdge> graph;

        public NodeTypeWalker(ToscaServiceTemplate toscaServiceTemplate, Action<string> action)
        {
            graph = new AdjacencyGraph<string, ToscaGraphEdge>();
            graph.AddVertexRange(toscaServiceTemplate.NodeTypes.Select(_ => _.Key));
            foreach (var toscaNodeType in toscaServiceTemplate.NodeTypes)
            {
                if (!toscaNodeType.Value.IsRoot())
                {
                    graph.AddEdge(new ToscaGraphEdge(toscaNodeType.Value.DerivedFrom, toscaNodeType.Key));
                }
            }

            this.action = action;
        }

        public void Walk(string rootNodeType)
        {
            var breadthFirstSearchAlgorithm = new BreadthFirstSearchAlgorithm<string,ToscaGraphEdge>(graph);
            breadthFirstSearchAlgorithm.DiscoverVertex += breadthFirstSearchAlgorithm_DiscoverVertex;
            breadthFirstSearchAlgorithm.Compute(rootNodeType);
        }

        void breadthFirstSearchAlgorithm_DiscoverVertex(string vertex)
        {
            action(vertex);
        }
    }

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

    //public class Graph<TK,TV> 
    //{

    //    private readonly List<Node> nodes = new List<Node>();

    //    public IEnumerable<Node> Nodes
    //    {
    //        get { return this.nodes; }
    //    }
    //}

    //internal class Node
    //{
    //}
}