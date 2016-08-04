using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace Toscana.Engine
{
    internal class ToscaCloudServiceArchiveWalker
    {
        private readonly Action<ToscaNodeType> action;
        private readonly AdjacencyGraph<string, ToscaGenericGraphEdge> graph;
        private readonly IReadOnlyDictionary<string, ToscaNodeType> nodeTypes;

        public ToscaCloudServiceArchiveWalker(ToscaCloudServiceArchive cloudServiceArchive, Action<ToscaNodeType> action)
        {
            nodeTypes = cloudServiceArchive.NodeTypes;
            graph = new AdjacencyGraph<string, ToscaGenericGraphEdge>();
            graph.AddVertexRange(cloudServiceArchive.NodeTypes.Select(_ => _.Key));
            foreach (var toscaNodeType in cloudServiceArchive.NodeTypes)
            {
                if (!toscaNodeType.Value.IsRoot())
                {
                    graph.AddEdge(new ToscaGenericGraphEdge(
                        "derived_from",
                        toscaNodeType.Value.DerivedFrom, 
                        toscaNodeType.Key));
                }
                foreach (var requirementKeyValue in toscaNodeType.Value.Requirements.SelectMany(r=>r).ToArray())
                {
                    graph.AddEdge(new ToscaGenericGraphEdge(
                        "requirement",
                        toscaNodeType.Key,
                        requirementKeyValue.Value.Node));
                }
            }

            this.action = action;
        }

        public void Walk()
        {
            var breadthFirstSearchAlgorithm = new BreadthFirstSearchAlgorithm<string,ToscaGenericGraphEdge>(graph);
            breadthFirstSearchAlgorithm.DiscoverVertex += breadthFirstSearchAlgorithm_DiscoverVertex;
            breadthFirstSearchAlgorithm.Compute(ToscaDefaults.ToscaNodesRoot);
        }

        void breadthFirstSearchAlgorithm_DiscoverVertex(string vertex)
        {
            action(nodeTypes[vertex]);
        }
    }

    internal class ToscaGenericGraphEdge : IEdge<string>
    {
        private readonly string source;
        private readonly string target;
        private readonly string edgeType;

        public ToscaGenericGraphEdge(string edgeType, string source, string target)
        {
            this.edgeType = edgeType;
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

        public string EdgeType
        {
            get { return edgeType; }
        }
    }
}