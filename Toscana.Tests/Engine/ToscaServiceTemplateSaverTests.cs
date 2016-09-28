using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaServiceTemplateSaverTests
    {
        [Test]
        public void It_Is_Possible_To_Save_Service_Template_To_File()
        {
            var mockFileSystem = new MockFileSystem();
            var bootstrapper = new Bootstrapper();
            var serviceTemplateSaver = new ToscaServiceTemplateSaver(mockFileSystem,
                bootstrapper.GetToscaServiceTemplateSerializer());

            var serviceTemplate = new ToscaServiceTemplate
            {
                ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
            };

            serviceTemplateSaver.Save(@"c:\files\service_template.yml", serviceTemplate);

            mockFileSystem.FileExists(@"c:\files\service_template.yml").Should().BeTrue();
        }

        [Test]
        public void Saving_Smaller_File_After_Reading_It_Works_Properly()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory(@"C:\Dir\SubDir");
            var filePath = @"C:\Dir\SubDir\service_template.yml";

            var bootstrapper = new Bootstrapper().Replace<IFileSystem>(mockFileSystem);
            var serviceTemplateSaver = bootstrapper.GetToscaServiceTemplateSaver();
            var serviceTemplateLoader = bootstrapper.GetToscaServiceTemplateLoader();

            #region Prepare a YAML file with some definitions

            var serviceTemplate = new ToscaServiceTemplate
            {
                ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
            };
            var nodeType = new ToscaNodeType();
            nodeType.Properties.Add("name", new ToscaPropertyDefinition {Type = "string"});
            nodeType.Properties.Add("age", new ToscaPropertyDefinition {Type = "integer"});
            nodeType.Properties.Add("gender", new ToscaPropertyDefinition {Type = "boolean"});
            serviceTemplate.NodeTypes.Add("tosca.nodes.Simple", nodeType);

            serviceTemplateSaver.Save(filePath, serviceTemplate);

            #endregion

            // Act
            var loadedServiceTemplate = serviceTemplateLoader.Load(filePath);
            loadedServiceTemplate.NodeTypes.Clear();
            serviceTemplateSaver.Save(filePath, loadedServiceTemplate);

            Action action = () => serviceTemplateLoader.Load(filePath);

            action.ShouldNotThrow<ToscaBaseException>();
        }
    }
}