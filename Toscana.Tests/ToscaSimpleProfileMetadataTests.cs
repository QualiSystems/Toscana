

using System;
using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaSimpleProfileMetadataTests
    {
        [Test]
        public void Should_Be_Possible_To_Get_And_Set_Value_Of_TemplateAuthor()
        {
            var toscaSimpleProfileMetadata = new ToscaSimpleProfileMetadata
            {
                TemplateAuthor = "me"
            };

            toscaSimpleProfileMetadata.TemplateAuthor.Should().Be("me");
        }

        [Test]
        public void Should_Be_Possible_To_Get_And_Set_Value_Of_TemplateVersion()
        {
            var toscaSimpleProfileMetadata = new ToscaSimpleProfileMetadata
            {
                TemplateVersion = new Version("123.45")
            };

            toscaSimpleProfileMetadata.TemplateVersion.Should().Be(new Version("123.45"));
        }

        [Test]
        public void TemplateAuthor_Should_Be_Empty_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaSimpleProfileMetadata();

            toscaSimpleProfileMetadata.TemplateAuthor.Should().BeEmpty();
        }

        [Test]
        public void TemplateName_Should_Be_Empty_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaSimpleProfileMetadata();

            toscaSimpleProfileMetadata.TemplateName.Should().BeEmpty();
        }

        [Test]
        public void TemplateVersion_Should_Be_Null_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaSimpleProfileMetadata();

            toscaSimpleProfileMetadata.TemplateVersion.Should().BeNull();
        }
    }
}

