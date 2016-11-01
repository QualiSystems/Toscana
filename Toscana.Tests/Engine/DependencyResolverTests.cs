using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class DependencyResolverTests
    {
        [Test]
        public void GetToscaCloudServiceArchiveLoader_Should_Return_Not_Null()
        {
            var toscaCloudServiceArchiveLoader = DependencyResolver.GetToscaCloudServiceArchiveLoader();

            toscaCloudServiceArchiveLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateLoader_Should_Return_Not_Null()
        {
            var toscaServiceTemplateLoader = DependencyResolver.GetToscaServiceTemplateLoader();

            toscaServiceTemplateLoader.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateParser_Should_Return_Not_Null()
        {
            var toscaServiceTemplateParser = DependencyResolver.GetToscaServiceTemplateParser();

            toscaServiceTemplateParser.Should().NotBeNull();
        }

        [Test]
        public void GetToscaServiceTemplateSaver_Should_Return_Not_Null()
        {
            var serviceTemplateSaver = DependencyResolver.GetToscaServiceTemplateSaver();

            serviceTemplateSaver.Should().NotBeNull();
        }
    }
}