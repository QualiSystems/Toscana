using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaSimpleProfileBuilderTests
    {
        [Test]
        public void Test()
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            toscaSimpleProfileBuilder.Append(new ToscaSimpleProfile {ToscaDefinitionsVersion = ""});
            //TestCop		
            //Assert.Fail("WriteMe");
        }
    }
}