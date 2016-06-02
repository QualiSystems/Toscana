using FluentAssertions;
using NUnit.Framework;
using Toscana.Domain;

namespace Toscana.Tests.Domain
{
    [TestFixture]
    public class MemorySizeTests
    {
        [Test]
        public void MemorySize_10MB_ParsedToTotalBytes()
        {
            var memorySize = new MemorySize("10 MB");

            memorySize.TotalBytes.Should().Be(10485760);
        }

        [Test]
        public void MemorySize_IsBinarySerializable()
        {
            var memorySize = new MemorySize("10 MB");

            memorySize.Should().BeBinarySerializable();
        }
    }
}