using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaServiceTemplateLoaderTests
    {
        [Test]
        public void I_Can_Load_Service_Template_From_File()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile("some_defs.yaml", new MockFileData(@"
tosca_definitions_version: tosca_simple_yaml_1_0
description: Common definitions
node_types:
    common_node_type:
        properties:
          num_cpus:
            type: integer
"));
            var serviceTemplateLoader = new ToscaServiceTemplateLoader(fileSystem, DependencyResolver.GetToscaServiceTemplateParser());
            var serviceTemplate = serviceTemplateLoader.Load("some_defs.yaml");

            serviceTemplate.Description.Should().Be("Common definitions");
        }
    }
}