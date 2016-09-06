namespace Toscana.Engine
{
    internal class ToscaNullDataTypeConverter : IToscaDataTypeValueConverter
    {
        public bool CanConvert(string dataTypeName)
        {
            return dataTypeName == "null";
        }

        public bool TryParse(object dataTypeValue, out object result)
        {
            result = dataTypeValue;
            return true;
        }
    }
}