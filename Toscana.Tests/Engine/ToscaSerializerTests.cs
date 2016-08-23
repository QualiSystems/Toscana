using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Toscana.Engine;

namespace Toscana.Tests.Engine
{
    [TestFixture]
    public class ToscaSerializerTests
    {
        [Test]
        public void Service_Template_Can_Be_Serialized_And_Deserialized()
        {
            byte[] bytes;
            using (var readingStream = new MemoryStream())
            {
                var serviceTemplateSerializer = new ToscaSerializer<ToscaServiceTemplate>(new TypeConvertersFactory());

                var serviceTemplate = new ToscaServiceTemplate();
                serviceTemplate.ToscaDefinitionsVersion = "tosca_simple_yaml_1_0";
                serviceTemplate.NodeTypes.Add("node", new ToscaNodeType { Version = new Version(1, 2, 3, 4) });

                serviceTemplateSerializer.Serialize(readingStream, serviceTemplate);

                bytes = readingStream.GetBuffer();
            }

            var serviceTemplateDeserializer = ToscaDeserializerTests.CreateToscaDeserializer();
            using (var writingStream = new MemoryStream(bytes))
            {
                var deserializedServiceTemplate = serviceTemplateDeserializer.Deserialize(writingStream);

                // Assert
                deserializedServiceTemplate.NodeTypes["node"].Version.Should().Be(new Version(1, 2, 3, 4));
            }
        }
    }
}