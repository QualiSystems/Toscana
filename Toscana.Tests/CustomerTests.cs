using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;
using Toscana.Tests.Engine;

namespace Toscana.Tests
{
    [TestFixture]
    // ReSharper disable once TestFileNameWarning
    public class CustomerTests
    {
        [Test]
        public void aaa()
        {
            var mockFileSystem = new MockFileSystem();
            DependencyResolver.Current.Replace<IFileSystem>(mockFileSystem);

            mockFileSystem.AddFile(Path.Combine(Environment.CurrentDirectory, "standard.yaml"),
                new MockFileData(@"
tosca_definitions_version: tosca_simple_yaml_1_0"));

            var archiveContent = new[]
            {
                new FileContent("TOSCA.meta",
                    @"
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: OASIS TOSCA TC
Entry-Definitions: definition.yaml
"),

                new FileContent("definition.yaml",
@"
tosca_definitions_version: tosca_simple_yaml_1_0
imports:
  - standard: standard.yaml

node_types:
  vendor.switch.NXOS:
    description: Description of NXOS switch
    derived_from: tosca.nodes.Root
    properties:
      device_owner:
        type: string
")
            };

            mockFileSystem.CreateArchive("tosca.zip", archiveContent);

            // Act
            var toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("tosca.zip", Environment.CurrentDirectory);

            // Assert
            toscaCloudServiceArchive.NodeTypes.Should().ContainSingle(a => a.Key == "vendor.switch.NXOS");

        }
    }
}