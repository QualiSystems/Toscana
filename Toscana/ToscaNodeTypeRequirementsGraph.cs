using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using Toscana.Engine;

namespace Toscana
{
    internal class ToscaNodeTypeRequirementsGraph
    {
        private readonly AdjacencyGraph<string, ToscaGraphEdge> graph;
        private readonly IReadOnlyDictionary<string, ToscaNodeType> nodeTypes;

        public ToscaNodeTypeRequirementsGraph(ToscaCloudServiceArchive cloudServiceArchive)
        {
            nodeTypes = cloudServiceArchive.NodeTypes;
            graph = new AdjacencyGraph<string, ToscaGraphEdge>();
            graph.AddVertexRange(cloudServiceArchive.NodeTypes.Select(_ => _.Key));
            foreach (var toscaNodeType in cloudServiceArchive.NodeTypes)
            {
                if (toscaNodeType.Value.IsRoot()) continue;

                foreach (var requirement in toscaNodeType.Value.GetAllRequirements())
                {
                    if (requirement.Node == null || 
                        requirement.Node == ToscaDefaults.ToscaNodesRoot ||
                        !nodeTypes.ContainsKey(requirement.Node))
                    {
                        continue;
                    }

                    graph.AddEdge(new ToscaGraphEdge(toscaNodeType.Key, requirement.Node));
                }
            }
        }

        public void Walk(string nodeTypeNameToStart, Action<string, ToscaNodeType> action)
        {
            var breadthFirstSearchAlgorithm = new BreadthFirstSearchAlgorithm<string, ToscaGraphEdge>(graph);
            breadthFirstSearchAlgorithm.DiscoverVertex +=
                nodeTypeName => { action(nodeTypeName, nodeTypes[nodeTypeName]); };
            breadthFirstSearchAlgorithm.Compute(nodeTypeNameToStart);
        }

        public bool ContainsCyclicLoop()
        {
            try
            {
                graph.TopologicalSort();
                return false;
            }
            catch (NonAcyclicGraphException)
            {
                return true;
            }
        }
    }
}