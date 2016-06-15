using System;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;
using Toscana.Exceptions;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaSimpleProfileBuilderTests
    {
        [Test]
        public void Build_Capabilities_Of_Base_And_Derived_Node_Types_Are_Merged()
        {
            // Arrange
            var nodeType = new NodeType();
            nodeType.Capabilities.Add("base_capability1", "base_capability1_type");
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new NodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Capabilities.Add("capability1", "capability1_type");
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            toscaSimpleProfileBuilder.Append(baseProfile);
            toscaSimpleProfileBuilder.Append(derivedProfile);
            var combinedToscaProfile = toscaSimpleProfileBuilder.Build();

            // Assert
            var combinedNodeType = combinedToscaProfile.NodeTypes["node1"];
            combinedNodeType.Capabilities.Should().HaveCount(2);
            combinedNodeType.Capabilities["capability1"].Type.Should().Be("capability1_type");
            combinedNodeType.Capabilities["base_capability1"].Type.Should().Be("base_capability1_type");
        }

        [Test]
        public void Exception_Thrown_When_Base_NodeType_Is_Missing()
        {
            // Arrange
            var derivedNodeType = new NodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Capabilities.Add("capability1", "capability1_type");
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            toscaSimpleProfileBuilder.Append(derivedProfile);
            Action action = () => toscaSimpleProfileBuilder.Build();

            // Assert
            action.ShouldThrow<ToscaValidationException>().WithMessage("Definition of Node Type base_node is missing");
        }
    }
}