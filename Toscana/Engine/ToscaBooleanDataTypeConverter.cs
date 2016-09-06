namespace Toscana.Engine
{
    internal class ToscaBooleanDataTypeConverter : IToscaDataTypeValueConverter
    {
        public bool CanConvert(string dataTypeName)
        {
            return dataTypeName == "boolean";
        }

        public bool TryParse(object dataTypeValue, out object result)
        {
            if (dataTypeValue == null)
            {
                result = null;
                return false;
            }
            bool valueAsBoolean;
            var canParse = bool.TryParse(dataTypeValue.ToString(), out valueAsBoolean);
            result = valueAsBoolean;
            return canParse;
        }
    }
}