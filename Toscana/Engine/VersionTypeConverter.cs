using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Version = System.Version;

namespace Toscana.Engine
{
    internal class VersionTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof (Version);
        }

        /// <summary>
        /// Reads version value
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException">At least one component of version represents a number greater than <see cref="F:System.Int32.MaxValue" />.</exception>
        public object ReadYaml(IParser parser, Type type)
        {
            var value = ((Scalar)parser.Current).Value;
            parser.MoveNext();
            return new Version(value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var version = (Version)value;
            emitter.Emit(new Scalar(null, null, version.ToString(), ScalarStyle.Any, true, false));
        }
    }
}