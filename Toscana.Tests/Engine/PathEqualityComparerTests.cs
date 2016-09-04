using FluentAssertions;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class PathEqualityComparerTests
    {
        [Test]
        [TestCase("file.yaml", "file.yaml", true)]
        [TestCase("file.yaml", "FiLe.yaMl", true)]
        [TestCase("dir\\file.yaml", "dir/file.yaml", true)]
        [TestCase("dir\\file.yaml", "dir/FILE.yaml", true)]
        [TestCase("file.yaml", "fail.yaml", false)]
        public void Equals_Should_Be_Case_Insensitive_And_Dir_Separator_Agnostic(string x, string y, bool result)
        {
            var pathEqualityComparer = new PathEqualityComparer();
            pathEqualityComparer.Equals(x, y).Should().Be(result);
        }
    }
}