using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaObjectTests
    {
        [Test]
        public void IsRoot_Should_Be_True_When_DerivedFrom_Not_Initialized()
        {
            var toscaObject = new TestToscaObject();

            toscaObject.IsRoot().Should().BeTrue();
        }

        [Test]
        public void IsRoot_Should_Be_False_When_DerivedFrom_Not_Empty()
        {
            var toscaObject = new TestToscaObject()
            {
                DerivedFrom = "base"
            };

            toscaObject.IsRoot().Should().BeFalse();
        }

        public class TestToscaObject : ToscaObject<TestToscaObject>
        {
            public override TestToscaObject GetDerivedFromEntity()
            {
                throw new System.NotImplementedException();
            }

            public override void SetDerivedFromToRoot(string name)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}