using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaNetAnalyzerTests
    {
        [Test]
        public void Analyze_HelloWorld_AllDataAnalyzed()
        {
            const string toscaString = @"
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

            var tosca = new ToscaNetAnalyzer().Analyze(toscaString);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying a single server with predefined properties.");
            var nodeTemplate = tosca.TopologyTemplate.NodeTemplates["my_server"];
            nodeTemplate.Type.Should().Be("tosca.nodes.Compute");

            var host = nodeTemplate.Capabilities.Host;
            host.Properties.NumCpus.Should().Be("1");
            host.Properties.DiskSize.Should().Be(new DigitalStorage("10 GB"));
            host.Properties.MemSize.Should().Be(new DigitalStorage("4 GB"));

            var os = nodeTemplate.Capabilities.Os;
            os.Properties.Architecture.Should().Be("x86_64");
            os.Properties.Type.Should().Be("linux");
            os.Properties.Distribution.Should().Be("rhel");
            os.Properties.Version.Should().Be("6.5");
        }

        [Test]
        public void Analyze_Overriding_Behavior_of_Predefined_Node_Types()
        {
            const string toscaString = @"tosca_definitions_version: tosca_simple_yaml_1_0
 
description: Template for deploying a single server with MySQL software on top.
 
topology_template:
  inputs:
    # omitted here for brevity
 
  node_templates:
    mysql:
      type: tosca.nodes.DBMS.MySQL
      properties:
        root_password: { get_input: my_mysql_rootpw }
        port: { get_input: my_mysql_port }
      requirements:
        - host: db_server
      interfaces:
        Standard:
          configure: scripts/my_own_configure.sh
 
    db_server:
      type: tosca.nodes.Compute
      capabilities:
        # omitted here for brevity";

            var tosca = new ToscaNetAnalyzer().Analyze(toscaString);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying a single server with MySQL software on top.");
            var topologyTemplate = tosca.TopologyTemplate;

            topologyTemplate.Inputs.Should().BeNull();
            topologyTemplate.Outputs.Should().BeNull();

            topologyTemplate.NodeTemplates.Should().HaveCount(2);

            var mysqlNodeTemplate = topologyTemplate.NodeTemplates["mysql"];
            mysqlNodeTemplate.Type.Should().Be("tosca.nodes.DBMS.MySQL");
            var requirementKeyValue = mysqlNodeTemplate.Requirements.Single().Single();
            requirementKeyValue.Key.Should().Be("host");
            requirementKeyValue.Value.Should().Be("db_server");
            var standardInterface = (IDictionary<object, object>) mysqlNodeTemplate.Interfaces["Standard"];
            standardInterface["configure"].Should().Be("scripts/my_own_configure.sh");

            var dbServerNodeTemplate = topologyTemplate.NodeTemplates["db_server"];
            dbServerNodeTemplate.Type.Should().Be("tosca.nodes.Compute");
            dbServerNodeTemplate.Capabilities.Should().BeNull();
            dbServerNodeTemplate.Requirements.Should().BeNull();
        }

        [Test]
        public void Analyze_Template_For_a_Simple_Software_Installation()
        {
            const string toscaString = @"tosca_definitions_version: tosca_simple_yaml_1_0
description: Template for deploying a single server with MySQL software on top.
 
topology_template:
  inputs:
    # omitted here for brevity
 
  node_templates:
    mysql:
      type: tosca.nodes.DBMS.MySQL
      properties:
        root_password: { get_input: my_mysql_rootpw }
        port: { get_input: my_mysql_port }
      requirements:
        - host: db_server
 
    db_server:
      type: tosca.nodes.Compute
      capabilities:
        # omitted here for brevity";

            var tosca = new ToscaNetAnalyzer().Analyze(toscaString);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying a single server with MySQL software on top.");
            var topologyTemplate = tosca.TopologyTemplate;

            topologyTemplate.Inputs.Should().BeNull();
            topologyTemplate.Outputs.Should().BeNull();

            topologyTemplate.NodeTemplates.Should().HaveCount(2);

            var mysqlNodeTemplate = topologyTemplate.NodeTemplates["mysql"];
            mysqlNodeTemplate.Type.Should().Be("tosca.nodes.DBMS.MySQL");
            var requirementKeyValue = mysqlNodeTemplate.Requirements.Single().Single();
            requirementKeyValue.Key.Should().Be("host");
            requirementKeyValue.Value.Should().Be("db_server");

            var dbServerNodeTemplate = topologyTemplate.NodeTemplates["db_server"];
            dbServerNodeTemplate.Type.Should().Be("tosca.nodes.Compute");
            dbServerNodeTemplate.Capabilities.Should().BeNull();
            dbServerNodeTemplate.Requirements.Should().BeNull();
        }

        [Test]
        public void Analyze_Template_For_Database_Content_Deployment()
        {
            const string toscaString = @"tosca_definitions_version: tosca_simple_yaml_1_0
 
description: Template for deploying MySQL and database content.
 
topology_template:
  inputs:
    # omitted here for brevity
 
  node_templates:
    my_db:
      type: tosca.nodes.Database.MySQL
      properties:
        name: { get_input: database_name }
        user: { get_input: database_user }
        password: { get_input: database_password }
        port: { get_input: database_port }
      artifacts:
        db_content:
          file: files/my_db_content.txt
          type: tosca.artifacts.File
      requirements:
        - host: mysql
      interfaces:
        Standard:
          create:
            implementation: db_create.sh
            inputs:
              # Copy DB file artifact to server’s staging area
              db_data: { get_artifact: [ SELF, db_content ] }
 
    mysql:
      type: tosca.nodes.DBMS.MySQL
      properties:
        root_password: { get_input: mysql_rootpw }
        port: { get_input: mysql_port }
      requirements:
        - host: db_server
 
    db_server:
      type: tosca.nodes.Compute
      capabilities:
        # omitted here for brevity";

            var tosca = new ToscaNetAnalyzer().Analyze(toscaString);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying MySQL and database content.");
            var topologyTemplate = tosca.TopologyTemplate;

            topologyTemplate.Inputs.Should().BeNull();
            topologyTemplate.Outputs.Should().BeNull();

            topologyTemplate.NodeTemplates.Should().HaveCount(3);

            var mysqlNodeTemplate = topologyTemplate.NodeTemplates["mysql"];
            mysqlNodeTemplate.Type.Should().Be("tosca.nodes.DBMS.MySQL");
            var requirementKeyValue = mysqlNodeTemplate.Requirements.Single().Single();
            requirementKeyValue.Key.Should().Be("host");
            requirementKeyValue.Value.Should().Be("db_server");

            var dbServerNodeTemplate = topologyTemplate.NodeTemplates["db_server"];
            dbServerNodeTemplate.Type.Should().Be("tosca.nodes.Compute");
            dbServerNodeTemplate.Capabilities.Should().BeNull();
            dbServerNodeTemplate.Requirements.Should().BeNull();

            var myDbNodeTemplate = topologyTemplate.NodeTemplates["my_db"];
            myDbNodeTemplate.Type.Should().Be("tosca.nodes.Database.MySQL");
            myDbNodeTemplate.Capabilities.Should().BeNull();
            myDbNodeTemplate.Requirements.Single()["host"].Should().Be("mysql");
            myDbNodeTemplate.Properties.Should().HaveCount(4);

            myDbNodeTemplate.Artifacts.Should().HaveCount(1);
            var artifact = myDbNodeTemplate.Artifacts["db_content"];
            artifact.File.Should().Be("files/my_db_content.txt");
            artifact.Type.Should().Be("tosca.artifacts.File");
        }

        [Test]
        public void Analyze_With_Inputs_AllDataAnalyzed()
        {
            const string toscaString = @"tosca_definitions_version: tosca_simple_yaml_1_0
 
description: Template for deploying a single server with predefined properties.
 
topology_template:
  inputs:
    cpus:
      type: integer
      description: Number of CPUs for the server.
      constraints:
        - valid_values: [ 1, 2, 4, 8 ]
 
  node_templates:
    my_server:
      type: tosca.nodes.Compute
      capabilities:
        # Host container properties
        host:
          properties:
            # Compute properties
            num_cpus: { get_input: cpus }
            mem_size: 2048  MB
            disk_size: 10 GB
 
  outputs:
    server_ip:
      description: The private IP address of the provisioned server.
      value: { get_attribute: [ my_server, private_address ] }";

            var tosca = new ToscaNetAnalyzer().Analyze(toscaString);

            // Assert
            tosca.ToscaDefinitionsVersion.Should().Be("tosca_simple_yaml_1_0");
            tosca.Description.Should().Be("Template for deploying a single server with predefined properties.");
            var topologyTemplate = tosca.TopologyTemplate;

            var topologyInputCpus = topologyTemplate.Inputs["cpus"];
            topologyInputCpus.Type.Should().Be("integer");
            topologyInputCpus.Description.Should().Be("Number of CPUs for the server.");
            topologyInputCpus.Constraints.Single()["valid_values"].ShouldAllBeEquivalentTo(new[] {1, 2, 4, 8});

            var topologyOutput = topologyTemplate.Outputs["server_ip"];
            topologyOutput.Description.Should().Be("The private IP address of the provisioned server.");
            var getAttributeValue = ((IDictionary<object, object>) topologyOutput.Value)["get_attribute"];
            ((List<object>) getAttributeValue).ShouldAllBeEquivalentTo(new[] {"my_server", "private_address"});

            var nodeTemplate = topologyTemplate.NodeTemplates["my_server"];
            nodeTemplate.Type.Should().Be("tosca.nodes.Compute");

            nodeTemplate.Type.Should().Be("tosca.nodes.Compute");
            nodeTemplate.Capabilities.Os.Should().BeNull();
            var hostProperties = nodeTemplate.Capabilities.Host.Properties;
            ((IDictionary<object, object>) hostProperties.NumCpus)["get_input"].Should().Be("cpus");
            hostProperties.MemSize.Should().Be(new DigitalStorage("2 GB"));
            hostProperties.DiskSize.Should().Be(new DigitalStorage("10 GB"));
        }
    }
}