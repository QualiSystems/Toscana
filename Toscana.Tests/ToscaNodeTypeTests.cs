using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaNodeTypeTests
    {
        [Test]
        public void Capabilities_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Capabilities.Should().HaveCount(0);
        }

        [Test]
        public void Artifacts_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Artifacts.Should().HaveCount(0);
        }

        [Test]
        public void Attributes_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Attributes.Should().HaveCount(0);
        }

        [Test]
        public void Interfaces_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Interfaces.Should().HaveCount(0);
        }

        [Test]
        public void Properties_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Properties.Should().HaveCount(0);
        }

        [Test]
        public void Requirements_Should_Have_Count_Zero_After_Initialization()
        {
            var nodeType = new ToscaNodeType();

            nodeType.Requirements.Should().HaveCount(0);
        }

        [Test]
        public void AddRequirement_Requirement_Exist()
        {
            // Arrange
            var toscaNodeType = new ToscaNodeType();
            var toscaRequirement = new ToscaRequirement()
            {
                Node = "port"
            };

            // Act
            toscaNodeType.AddRequirement("device", toscaRequirement);

            // Assert
            toscaNodeType.Requirements.Single()["device"].Node.Should().Be("port");
        }
    }
}