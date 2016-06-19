using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class ToscaObjectTests
    {
        [Test]
        public void IsRoot_Should_Be_True_When_DerivedFrom_Not_Initialized()
        {
            var toscaObject = new ToscaObject();

            toscaObject.IsRoot().Should().BeTrue();
        }

        [Test]
        public void IsRoot_Should_Be_False_When_DerivedFrom_Not_Empty()
        {
            var toscaObject = new ToscaObject()
            {
                DerivedFrom = "base"
            };

            toscaObject.IsRoot().Should().BeFalse();
        }
    }
}