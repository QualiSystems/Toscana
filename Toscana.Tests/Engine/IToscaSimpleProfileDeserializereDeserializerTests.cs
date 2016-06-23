﻿using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class IToscaSimpleProfileDeserializereDeserializerTests
    {
        [Test]
        public void Parse_Template_With_Single_Requirement()
        {
            // Arrange
            var toscaParser = new ToscaSimpleProfileDeserializer();
            // Act
            var tosca = toscaParser.Deserialize(@"
tosca_definitions_version: tosca_simple_yaml_1_0

node_types:

  cloudshell.networking.nodes.NetworkDevice:
    derived_from: cloudshell.standard.Shell
    requirements:
        - Chassis:
            capability: tosca.capabilities.Attachment
            node: cloudshell.nodes.GenericChassis
            relationship: tosca.relationships.AttachesTo
            occurrences: [0, UNBOUNDED]
        - Port Channels:
            capability: tosca.capabilities.Attachment
            node: cloudshell.nodes.GenericPortChannel
            relationship: tosca.relationships.AttachesTo
            occurrences: [0, UNBOUNDED]
");

            // Assert
            tosca.NodeTypes.Should().HaveCount(1);
            var nodeType = tosca.NodeTypes["cloudshell.networking.nodes.NetworkDevice"];
            nodeType.DerivedFrom.Should().Be("cloudshell.standard.Shell");
            nodeType.Requirements.Should().HaveCount(2);
            var chassisRequirement = nodeType.Requirements.First()["Chassis"];
            chassisRequirement.Capability.Should().Be("tosca.capabilities.Attachment");
            chassisRequirement.Node.Should().Be("cloudshell.nodes.GenericChassis");
            chassisRequirement.Relationship.Should().Be("tosca.relationships.AttachesTo");

            var portChannelsRequirement = nodeType.Requirements.Last()["Port Channels"];
            portChannelsRequirement.Capability.Should().Be("tosca.capabilities.Attachment");
            portChannelsRequirement.Node.Should().Be("cloudshell.nodes.GenericPortChannel");
            portChannelsRequirement.Relationship.Should().Be("tosca.relationships.AttachesTo");
        }

        [Test]
        public void ToscaParsingException_Should_Be_Thrown_When_Wrong_Tosca_Parsed()
        {
            var toscaDeserializer = new ToscaSimpleProfileDeserializer();
            Action action = () => toscaDeserializer.Deserialize(@"
tosca_definitions_version: tosca_simple_yaml_1_0
unsupported_something:");

            action.ShouldThrow<ToscaParsingException>()
                .WithMessage("(Line: 2, Col: 1, Idx: 2) - (Line: 2, Col: 1, Idx: 2): Exception during deserialization");
        }
    }
}