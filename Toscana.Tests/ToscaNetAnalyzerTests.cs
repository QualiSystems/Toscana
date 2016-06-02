using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaNetAnalyzerTests
    {
        private const string ToscaHelloWorld = @"
tosca_definitions_version: tosca_simple_yaml_1_0
 
description: Template for deploying a single server with predefined properties.
 
topology_template:
  node_templates:
    my_server:
      type: tosca.nodes.Compute
      capabilities:
        # Host container properties
        host:
         properties:
           num_cpus: 1
           disk_size: 10 GB
           mem_size: 4096 MB
        # Guest Operating System properties
        os:
          properties:
            # host Operating System image properties
            architecture: x86_64
            type: linux 
            distribution: rhel 
            version: 6.5 ";

        [Test]
        public void Analyze_HelloWorld_AllDataAnalyzed()
        {
            // Arrange
            var toscaNetAnalyzer = new ToscaNetAnalyzer();

            // Act
            var tosca = toscaNetAnalyzer.Analyze(ToscaHelloWorld);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying a single server with predefined properties.");
            var nodeTemplate = tosca.TopologyTemplate.NodeTemplates["my_server"];
            nodeTemplate.Type.Should().Be("tosca.nodes.Compute");

            var host = nodeTemplate.Capabilities["host"];
            host.Properties["num_cpus"].Should().Be("1");
            host.Properties["disk_size"].Should().Be("10 GB");
            host.Properties["mem_size"].Should().Be("4096 MB");

            var os = nodeTemplate.Capabilities["os"];
            os.Properties["architecture"].Should().Be("x86_64");
            os.Properties["type"].Should().Be("linux");
            os.Properties["distribution"].Should().Be("rhel");
            os.Properties["version"].Should().Be("6.5");
        }
    }
}