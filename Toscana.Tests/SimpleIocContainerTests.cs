using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class SimpleIocContainerTests
    {
        [Test]
        public void Second_Registration_Should_Override_The_First_One()
        {
            // Arrange
            var poorManContainer = new SimpleIocContainer();
            poorManContainer.Register<IFileSystem, FileSystem>();
            poorManContainer.RegisterSingleton<IFileSystem>(new MockFileSystem());

            // Act
            var fileSystem = poorManContainer.GetInstance<IFileSystem>();

            // Assert
            fileSystem.Should().BeOfType<MockFileSystem>();
        }
    }
}