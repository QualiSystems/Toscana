using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaPropertyTests
    {
        [Test]
        public void StringValue_Should_Be_Empty_When_Default_Is_Null()
        {
            var toscaPropertyDefinition = new ToscaProperty {Default = null};

            toscaPropertyDefinition.StringValue.Should().BeEmpty();
        }

        [Test]
        public void StringValue_Should_Be_Empty_When_Default_Is_StringEmpty()
        {
            var toscaPropertyDefinition = new ToscaProperty {Default = string.Empty};

            toscaPropertyDefinition.StringValue.Should().BeEmpty();
        }

        [Test]
        public void StringValue_Should_Be_Number_As_String_When_Default_Is_Integer()
        {
            var toscaPropertyDefinition = new ToscaProperty {Default = 23};

            toscaPropertyDefinition.StringValue.Should().Be("23");
        }

        [Test]
        public void Contraints_Should_Be_Empty_List_Upon_Initialization()
        {
            // Act
            var toscaPropertyDefinition = new ToscaProperty();

            // Assert
            toscaPropertyDefinition.Constraints.Should().BeEmpty();
        }
    }
}