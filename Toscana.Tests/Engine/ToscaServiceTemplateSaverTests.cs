using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

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
    }
}