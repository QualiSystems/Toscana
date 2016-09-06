namespace Toscana.Engine
{
    internal class ToscaIntegerDataTypeConverter : IToscaDataTypeValueConverter
    {
        public bool CanConvert(string dataTypeName)
        {
            return dataTypeName == "integer";
        }

        public bool TryParse(object dataTypeValue, out object result)
        {
            if (dataTypeValue == null)
            {
                result = null;
                return false;
            }
            int valueAsInteger;
            var canParse = int.TryParse(dataTypeValue.ToString(), out valueAsInteger);
            result = valueAsInteger;
            return canParse;
        }
    }
}