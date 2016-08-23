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
            using (var stream = new MemoryStream())
            {
                var serviceTemplateSerializer = new ToscaSerializer<ToscaServiceTemplate>(new TypeConvertersFactory());
                var serviceTemplateDeserializer = ToscaDeserializerTests.CreateToscaDeserializer();

                var serviceTemplate = new ToscaServiceTemplate();
                serviceTemplate.ToscaDefinitionsVersion = "tosca_simple_yaml_1_0";
                serviceTemplate.NodeTypes.Add("node", new ToscaNodeType { Version = new Version(1, 2, 3, 4) });

                using (var writer = new StreamWriter(stream))
                {
                    serviceTemplateSerializer.Serialize(writer, serviceTemplate);
                    writer.Flush();
                    stream.Position = 0;
                    var deserializedServiceTemplate = serviceTemplateDeserializer.Deserialize(stream);

                    // Assert
                    deserializedServiceTemplate.NodeTypes["node"].Version.Should().Be(new Version(1, 2, 3, 4));
                }
            }
        }
    }
}