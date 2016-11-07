using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace Toscana
{
    internal class ToscaNodeTypeInheritanceWalker
    {
        private readonly Action<string, ToscaNodeType> action;
        private readonly AdjacencyGraph<string, ToscaGraphEdge> graph;
        private readonly IReadOnlyDictionary<string, ToscaNodeType> nodeTypes;

        public ToscaNodeTypeInheritanceWalker(ToscaCloudServiceArchive cloudServiceArchive, Action<string, ToscaNodeType> action)
        {
            nodeTypes = cloudServiceArchive.NodeTypes;
            graph = new AdjacencyGraph<string, ToscaGraphEdge>();
            graph.AddVertexRange(cloudServiceArchive.NodeTypes.Select(_ => _.Key));
            foreach (var toscaNodeType in cloudServiceArchive.NodeTypes)
            {
                if (!toscaNodeType.Value.IsRoot())
                {
                    graph.AddEdge(new ToscaGraphEdge(
                        toscaNodeType.Value.DerivedFrom, 
                        toscaNodeType.Key));
                }
            }

            this.action = action;
        }

        public void Walk()
        {
            var breadthFirstSearchAlgorithm = new BreadthFirstSearchAlgorithm<string, ToscaGraphEdge>(graph);
            breadthFirstSearchAlgorithm.DiscoverVertex += nodeTypeName => { action(nodeTypeName, nodeTypes[nodeTypeName] ); };
            breadthFirstSearchAlgorithm.Compute(ToscaDefaults.ToscaNodesRoot);
        }
    }
}

