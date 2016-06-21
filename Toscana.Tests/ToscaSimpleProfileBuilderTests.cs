using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
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
            var nodeType = new ToscaNodeType();
            nodeType.Capabilities.Add("base_capability1", "base_capability1_type");
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Capabilities.Add("capability1", "capability1_type");
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var combinedToscaProfile = new ToscaSimpleProfileBuilder()
                .Append(baseProfile)
                .Append(derivedProfile)
                .Build();

            // Assert
            var combinedNodeType = combinedToscaProfile.NodeTypes["node1"];
            combinedNodeType.Capabilities.Should().HaveCount(2);
            combinedNodeType.Capabilities["capability1"].Type.Should().Be("capability1_type");
            combinedNodeType.Capabilities["base_capability1"].Type.Should().Be("base_capability1_type");
        }

        [Test]
        public void Properties_Of_Base_And_Derived_Node_Types_Are_Merged()
        {
            // Arrange
            var nodeType = new ToscaNodeType();
            nodeType.Properties.Add("base_property1", new ToscaPropertyDefinition {Type = "string"});
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Properties.Add("property1", new ToscaPropertyDefinition { Type = "int"});
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var combinedToscaProfile = new ToscaSimpleProfileBuilder()
                .Append(baseProfile)
                .Append(derivedProfile)
                .Build();

            // Assert
            var combinedNodeType = combinedToscaProfile.NodeTypes["node1"];
            combinedNodeType.Properties.Should().HaveCount(2);
            combinedNodeType.Properties["base_property1"].Type.Should().Be("string");
            combinedNodeType.Properties["property1"].Type.Should().Be("int");
        }

        [Test]
        public void Interfaces_Of_Base_And_Derived_Node_Types_Are_Merged()
        {
            // Arrange
            var nodeType = new ToscaNodeType();
            nodeType.Interfaces.Add("base_interface1", new Dictionary<string, object> { { "method1", "code" } });
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Interfaces.Add("interface1", new Dictionary<string, object>{{"method2", "code"}});
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var combinedToscaProfile = new ToscaSimpleProfileBuilder()
                .Append(baseProfile)
                .Append(derivedProfile)
                .Build();

            // Assert
            var combinedNodeType = combinedToscaProfile.NodeTypes["node1"];
            combinedNodeType.Interfaces.Should().HaveCount(2);
            combinedNodeType.Interfaces["base_interface1"]["method1"].Should().Be("code");
            combinedNodeType.Interfaces["interface1"]["method2"].Should().Be("code");
        }

        [Test]
        public void Requirements_Of_Base_And_Derived_Node_Types_Are_Merged()
        {
            // Arrange
            var nodeType = new ToscaNodeType();
            nodeType.Requirements.Add(new Dictionary<string, ToscaRequirement> { { "base_requirement1", new ToscaRequirement {Capability = "attachment1"} } });
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            nodeType.Requirements.Add(new Dictionary<string, ToscaRequirement> { { "requirement1", new ToscaRequirement { Capability = "attachment2" } } });
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var combinedToscaProfile = new ToscaSimpleProfileBuilder()
                .Append(baseProfile)
                .Append(derivedProfile)
                .Build();

            // Assert
            var combinedNodeType = combinedToscaProfile.NodeTypes["node1"];
            combinedNodeType.Requirements.Should().HaveCount(2);
            combinedNodeType.Requirements.Single(r=>r.ContainsKey("base_requirement1"))["base_requirement1"].Capability.Should().Be("attachment1");
            combinedNodeType.Requirements.Single(r => r.ContainsKey("requirement1"))["requirement1"].Capability.Should().Be("attachment2");
        }

        /// <summary>
        /// node_types:
        ///      Root
        ///     /    
        ///    A     
        ///   / \
        ///  B   C
        /// </summary>
        [Test]
        public void Node_Types_Capabilities_Should_Be_Successfully_Parsed_With_Several_Leaves_With_Same_Base_Node_Types()
        {
            // Arrange
            var rootNode = new ToscaNodeType();
            rootNode.Capabilities.Add("feature", "feature");
            var toscaSimpleProfile = new ToscaSimpleProfile();
            toscaSimpleProfile.NodeTypes.Add("root", rootNode);

            var nodeTypeA = new ToscaNodeType { DerivedFrom = "root" };
            toscaSimpleProfile.NodeTypes.Add("A", nodeTypeA);

            var nodeTypeB = new ToscaNodeType { DerivedFrom = "A" };
            toscaSimpleProfile.NodeTypes.Add("B", nodeTypeB);

            var nodeTypeC = new ToscaNodeType { DerivedFrom = "A" };
            toscaSimpleProfile.NodeTypes.Add("C", nodeTypeC);

            // Act
            var combinedToscaProfile = new ToscaSimpleProfileBuilder()
                .Append(toscaSimpleProfile)
                .Build();

            // Assert
            combinedToscaProfile.NodeTypes.Should().HaveCount(4);

            var rootNodeType = combinedToscaProfile.NodeTypes["root"];
            rootNodeType.Capabilities.Should().HaveCount(1);
            rootNodeType.Capabilities["feature"].Type.Should().Be("feature");

            var combinedNodeTypeA = combinedToscaProfile.NodeTypes["A"];
            combinedNodeTypeA.Capabilities.Should().HaveCount(1);
            combinedNodeTypeA.Capabilities["feature"].Type.Should().Be("feature");

            var combinedNodeTypeB = combinedToscaProfile.NodeTypes["B"];
            combinedNodeTypeB.Capabilities.Should().HaveCount(1);
            combinedNodeTypeB.Capabilities["feature"].Type.Should().Be("feature");

            var combinedNodeTypeC = combinedToscaProfile.NodeTypes["C"];
            combinedNodeTypeC.Capabilities.Should().HaveCount(1);
            combinedNodeTypeC.Capabilities["feature"].Type.Should().Be("feature");
        }

        [Test]
        public void Exception_Thrown_When_Duplicate_Capability_Appear_On_Base_And_Derived_Node_Type()
        {
            // Arrange
            var nodeType = new ToscaNodeType();
            nodeType.Capabilities.Add("capability1", "capability1_type");
            var baseProfile = new ToscaSimpleProfile();
            baseProfile.NodeTypes.Add("base_node", nodeType);

            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Capabilities.Add("capability1", "capability1_type");
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act

            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder()
                .Append(baseProfile)
                .Append(derivedProfile);

            Action action = () => toscaSimpleProfileBuilder.Build();

            // Assert
            action.ShouldThrow<ToscanaValidationException>().WithMessage("Duplicate capability definition of capability capability1");
        }

        [Test]
        public void Exception_Thrown_When_Duplicate_Node_Types_Appear()
        {
            // Arrange
            var nodeType = new ToscaNodeType();
            var profile1 = new ToscaSimpleProfile();
            profile1.NodeTypes.Add("duplicate_node", nodeType);

            var profile2 = new ToscaSimpleProfile();
            profile2.NodeTypes.Add("duplicate_node", nodeType);

            // Act
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder()
                .Append(profile1)
                .Append(profile2);
            Action action = () => toscaSimpleProfileBuilder.Build();

            // Assert
            action.ShouldThrow<ToscanaValidationException>().WithMessage("Node type duplicate_node is duplicate");
        }

        [Test]
        public void Exception_Thrown_When_Base_NodeType_Is_Missing()
        {
            // Arrange
            var derivedNodeType = new ToscaNodeType
            {
                DerivedFrom = "base_node"
            };
            derivedNodeType.Capabilities.Add("capability1", "capability1_type");
            var derivedProfile = new ToscaSimpleProfile();
            derivedProfile.NodeTypes.Add("node1", derivedNodeType);

            // Act
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder()
                .Append(derivedProfile);
            Action action = () => toscaSimpleProfileBuilder.Build();

            // Assert
            action.ShouldThrow<ToscanaValidationException>().WithMessage("Definition of Node Type base_node is missing");
        }
    }
}