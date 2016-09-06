namespace Toscana.Engine
{
    internal class ToscaFloatDataTypeConverter : IToscaDataTypeValueConverter
    {
        public bool CanConvert(string dataTypeName)
        {
            return dataTypeName == "float";
        }

        public bool TryParse(object dataTypeValue, out object result)
        {
            if (dataTypeValue == null)
            {
                result = null;
                return false;
            }
            float valueAsFloat;
            var canParse = float.TryParse(dataTypeValue.ToString(), out valueAsFloat);
            result = valueAsFloat;
            return canParse;
        }
    }
}