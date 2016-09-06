using System.Collections.Generic;
using System.Linq;

namespace Toscana.Engine
{
    internal interface IToscaParserFactory
    {
        IToscaDataTypeValueConverter GetParser(string type);
    }

    internal class ToscaParserFactory : IToscaParserFactory
    {
        private readonly IList<IToscaDataTypeValueConverter> converters;

        public ToscaParserFactory(IList<IToscaDataTypeValueConverter> converters)
        {
            this.converters = converters;
        }

        public IToscaDataTypeValueConverter GetParser(string type)
        {
            return converters.FirstOrDefault(c => c.CanConvert(type));
        }
    }
}