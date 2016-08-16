using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using Toscana.Engine;

namespace Toscana
{
    internal class ToscaNodeTypeRequirementsWalker
    {
        private readonly Action<string, ToscaNodeType> action;
        private readonly AdjacencyGraph<string, ToscaGraphEdge> graph;
        private readonly IReadOnlyDictionary<string, ToscaNodeType> nodeTypes;

        public ToscaNodeTypeRequirementsWalker(ToscaCloudServiceArchive cloudServiceArchive,
            Action<string, ToscaNodeType> action)
        {
            nodeTypes = cloudServiceArchive.NodeTypes;
            graph = new AdjacencyGraph<string, ToscaGraphEdge>();
            graph.AddVertexRange(cloudServiceArchive.NodeTypes.Select(_ => _.Key));
            foreach (var toscaNodeType in cloudServiceArchive.NodeTypes)
            {
                if (toscaNodeType.Value.IsRoot()) continue;

                foreach (var requirement in toscaNodeType.Value.GetAllRequirements().Values)
                {
                    if (requirement.Node == ToscaDefaults.ToscaNodesRoot) continue;

                    graph.AddEdge(new ToscaGraphEdge(toscaNodeType.Key, requirement.Node));
                }
            }

            this.action = action;
        }

        public void Walk(string nodeTypeNameToStart)
        {
            var breadthFirstSearchAlgorithm = new BreadthFirstSearchAlgorithm<string, ToscaGraphEdge>(graph);
            breadthFirstSearchAlgorithm.DiscoverVertex += nodeTypeName => { action(nodeTypeName, nodeTypes[nodeTypeName]); };
            breadthFirstSearchAlgorithm.Compute(nodeTypeNameToStart);
        }
    }
}