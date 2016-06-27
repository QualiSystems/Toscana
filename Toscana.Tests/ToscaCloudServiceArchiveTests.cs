using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaCloudServiceArchiveTests
    {
        [Test]
        public void ToscaMetadata_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive();

            // Assert
            toscaCloudServiceArchive.ToscaMetadata.Should().NotBeNull();
        }

        [Test]
        public void ToscaSimpleProfiles_Should_Not_Be_Null_Upon_Initialization()
        {
            // Act
            var toscaCloudServiceArchive = new ToscaCloudServiceArchive();

            // Assert
            toscaCloudServiceArchive.ToscaSimpleProfiles.Should().NotBeNull();
        }
    }
}