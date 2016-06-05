using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class DigitalStorageParserTests
    {
        [Test]
        public void Ctor_10MB_ParsedToTotalBytes()
        {
            var memorySize = new DigitalStorage("10 MB");

            memorySize.TotalBytes.Should().Be(10485760);
        }

        [Test]
        public void EqualsTo_10KB_EqualsTo_10240_Bytes()
        {
            var tenKB = new DigitalStorage("10 KB");

            var alsoTenKB = new DigitalStorage(10240);

            tenKB.Should().Be(alsoTenKB);
        }        
        
        [Test]
        public void ToString_10KB_FormattedAsString()
        {
            var tenKB = new DigitalStorage(10240);

            tenKB.ToString().Should().Be("10 KB");
        }

        [Test]
        public void Ctor_IsBinarySerializable()
        {
            var memorySize = new DigitalStorage("10 MB");

            memorySize.Should().BeBinarySerializable();
        }
    }
}