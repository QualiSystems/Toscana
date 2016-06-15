using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaSimpleProfileBuilderTests
    {
        [Test]
        public void Build_Capabilities_Of_Base_And_Derived_Node_Types_Are_Merged()
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            toscaSimpleProfileBuilder.Append(new ToscaSimpleProfile
            {
                NodeTypes = new Dictionary<string, NodeType>
                {
                    {
                        "base_node", new NodeType
                        {
                            Capabilities = new Dictionary<string, Capability>
                            {
                                {
                                    "base_capability1", new Capability
                                    {
                                        Type = "base_capability1_type"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            toscaSimpleProfileBuilder.Append(new ToscaSimpleProfile
            {
                NodeTypes = new Dictionary<string, NodeType>
                {
                    {
                        "node1", new NodeType
                        {
                            DerivedFrom = "base_node",
                            Capabilities = new Dictionary<string, Capability>
                            {
                                {
                                    "capability1", new Capability
                                    {
                                        Type = "capability1_type"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            var toscaSimpleProfile = toscaSimpleProfileBuilder.Build();

            // Assert
            var nodeType = toscaSimpleProfile.NodeTypes["node1"];
            nodeType.Capabilities.Should().HaveCount(2);
            nodeType.Capabilities["capability1"].Type.Should().Be("capability1_type");
            nodeType.Capabilities["base_capability1"].Type.Should().Be("base_capability1_type");
        }
    }
}