using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class NodeTypeTests
    {
        [Test]
        public void Capabilities_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Capabilities.Should().HaveCount(0);
        }

        [Test]
        public void Artifacts_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Artifacts.Should().HaveCount(0);
        }

        [Test]
        public void Attributes_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Attributes.Should().HaveCount(0);
        }

        [Test]
        public void Interfaces_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Interfaces.Should().HaveCount(0);
        }

        [Test]
        public void Properties_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Properties.Should().HaveCount(0);
        }

        [Test]
        public void Requirements_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new NodeType();

            nodeType.Requirements.Should().HaveCount(0);
        }
    }
}