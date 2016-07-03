using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaNodeTemplateTests
    {
        [Test]
        public void Artifacts_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Artifacts.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Properties.Should().BeEmpty();
        }

        [Test]
        public void Attributes_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Attributes.Should().BeEmpty();
        }

        [Test]
        public void Capabilities_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Capabilities.Should().BeEmpty();
        }

        [Test]
        public void Directives_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Directives.Should().BeEmpty();
        }

        [Test]
        public void Interfaces_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Interfaces.Should().BeEmpty();
        }

        [Test]
        public void Requirements_Should_Be_Empty_Upon_Initialization()
        {
            var toscaNodeTemplate = new ToscaNodeTemplate();

            toscaNodeTemplate.Requirements.Should().BeEmpty();
        }
    }
}