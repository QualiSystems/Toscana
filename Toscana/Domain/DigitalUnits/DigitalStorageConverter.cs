using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Toscana.Domain.DigitalUnits
{
    internal class DigitalStorageConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof (DigitalStorage);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            string g = ((Scalar)parser.Current).Value;
            parser.MoveNext();
            return new DigitalStorage(g);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            DigitalStorage digitalStorage = (DigitalStorage)value;
            emitter.Emit(new Scalar(digitalStorage.ToString()));
        }
    }
}