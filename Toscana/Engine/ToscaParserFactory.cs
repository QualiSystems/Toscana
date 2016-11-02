using System.Collections.Generic;
using System.Linq;

namespace Toscana.Engine
{
    internal interface IToscaDataTypeRegistry
    {
        IToscaDataTypeValueConverter GetConverter(string type);
        void Register(IToscaDataTypeValueConverter dataTypeValueConverter);
    }

    internal class ToscaDataTypeRegistry : IToscaDataTypeRegistry
    {
        private readonly IList<IToscaDataTypeValueConverter> converters;

        public ToscaDataTypeRegistry(IList<IToscaDataTypeValueConverter> converters)
        {
            this.converters = converters;
        }

        public IToscaDataTypeValueConverter GetConverter(string type)
        {
            return converters.FirstOrDefault(c => c.CanConvert(type));
        }

        public void Register(IToscaDataTypeValueConverter dataTypeValueConverter)
        {
            converters.Add(dataTypeValueConverter);
        }
    }
}