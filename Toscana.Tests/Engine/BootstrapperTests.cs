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
            var bootstrapper = new Bootstrapper();
            var toscaCloudServiceArchiveLoader = bootstrapper.GetToscaCloudServiceArchiveLoader();

            toscaCloudServiceArchiveLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateLoader_Should_Return_Not_Null()
        {
            var bootstrapper = new Bootstrapper();
            var toscaServiceTemplateLoader = bootstrapper.GetToscaServiceTemplateLoader();

            toscaServiceTemplateLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateParser_Should_Return_Not_Null()
        {
            var bootstrapper = new Bootstrapper();
            var toscaServiceTemplateParser = bootstrapper.GetToscaServiceTemplateParser();

            toscaServiceTemplateParser.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateSaver_Should_Return_Not_Null()
        {
            var bootstrapper = new Bootstrapper();
            var serviceTemplateSaver = bootstrapper.GetToscaServiceTemplateSaver();

            serviceTemplateSaver.Should().NotBeNull();
        }
    }
}