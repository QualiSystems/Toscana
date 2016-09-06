using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaRelationshipTemplateTests
    {
        [Test]
        public void Assignments_Should_Be_Empty_Upon_Initialization()
        {
            var toscaRelationshipTemplate = new ToscaRelationshipTemplate();

            toscaRelationshipTemplate.Assignments.Should().BeEmpty();
        }

        [Test]
        public void Interfaces_Should_Be_Empty_Upon_Initialization()
        {
            var toscaRelationshipTemplate = new ToscaRelationshipTemplate();

            toscaRelationshipTemplate.Interfaces.Should().BeEmpty();
        }

        [Test]
        public void Properties_Should_Be_Empty_Upon_Initialization()
        {
            var toscaRelationshipTemplate = new ToscaRelationshipTemplate();

            toscaRelationshipTemplate.Properties.Should().BeEmpty();
        }
    }
}