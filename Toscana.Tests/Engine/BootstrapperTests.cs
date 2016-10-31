using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class BootstrapperTests
    {
        [Test]
        public void GetToscaCloudServiceArchiveLoader_Should_Return_Not_Null()
        {
            var toscaCloudServiceArchiveLoader = Bootstrapper.GetToscaCloudServiceArchiveLoader();

            toscaCloudServiceArchiveLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateLoader_Should_Return_Not_Null()
        {
            var toscaServiceTemplateLoader = Bootstrapper.GetToscaServiceTemplateLoader();

            toscaServiceTemplateLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateParser_Should_Return_Not_Null()
        {
            var toscaServiceTemplateParser = Bootstrapper.GetToscaServiceTemplateParser();

            toscaServiceTemplateParser.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateSaver_Should_Return_Not_Null()
        {
            var serviceTemplateSaver = Bootstrapper.GetToscaServiceTemplateSaver();

            serviceTemplateSaver.Should().NotBeNull();
        }
    }
}