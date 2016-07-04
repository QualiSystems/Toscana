

using System;
using FluentAssertions;
using NUnit.Framework;

namespace Toscana.Tests
{
    [TestFixture]
    public class ToscaServiceTemplateMetadataTests
    {
        [Test]
        public void Should_Be_Possible_To_Get_And_Set_Value_Of_TemplateName()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata
            {
                TemplateName = "nut shell"
            };

            toscaSimpleProfileMetadata.TemplateName.Should().Be("nut shell");
        }

        [Test]
        public void Should_Be_Possible_To_Get_And_Set_Value_Of_TemplateAuthor()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata
            {
                TemplateAuthor = "me"
            };

            toscaSimpleProfileMetadata.TemplateAuthor.Should().Be("me");
        }

        [Test]
        public void Should_Be_Possible_To_Get_And_Set_Value_Of_TemplateVersion()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata
            {
                TemplateVersion = new Version("123.45")
            };

            toscaSimpleProfileMetadata.TemplateVersion.Should().Be(new Version("123.45"));
        }

        [Test]
        public void TemplateAuthor_Should_Be_Empty_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata();

            toscaSimpleProfileMetadata.TemplateAuthor.Should().BeEmpty();
        }

        [Test]
        public void TemplateName_Should_Be_Empty_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata();

            toscaSimpleProfileMetadata.TemplateName.Should().BeEmpty();
        }

        [Test]
        public void TemplateVersion_Should_Be_Null_If_Not_Set()
        {
            var toscaSimpleProfileMetadata = new ToscaServiceTemplateMetadata();

            toscaSimpleProfileMetadata.TemplateVersion.Should().BeNull();
        }
    }
}

