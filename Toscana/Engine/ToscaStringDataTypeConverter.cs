namespace Toscana.Engine
{
    internal class ToscaStringDataTypeConverter : IToscaDataTypeValueConverter
    {
        public bool CanConvert(string dataTypeName)
        {
            return dataTypeName == "string";
        }

        public bool TryParse(object dataTypeValue, out object result)
        {
            result = dataTypeValue ?? string.Empty;
            return true;
        }
    }
}