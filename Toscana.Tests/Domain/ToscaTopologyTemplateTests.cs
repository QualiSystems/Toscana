using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaTopologyTemplateTests
    {
        [Test]
        public void Inputs_Should_Be_Empty_Upon_Initialization()
        {
            var toscaTopologyTemplate = new ToscaTopologyTemplate();

            toscaTopologyTemplate.Inputs.Should().BeEmpty();
        }

        [Test]
        public void NodeTemplates_Should_Be_Empty_Upon_Initialization()
        {
            var toscaTopologyTemplate = new ToscaTopologyTemplate();

            toscaTopologyTemplate.NodeTemplates.Should().BeEmpty();
        }

        [Test]
        public void Outputs_Should_Be_Empty_Upon_Initialization()
        {
            var toscaTopologyTemplate = new ToscaTopologyTemplate();

            toscaTopologyTemplate.Outputs.Should().BeEmpty();
        }
    }
}