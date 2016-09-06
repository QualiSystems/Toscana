namespace Toscana.Engine
{
    internal interface IToscaDataTypeValueConverter
    {
        bool CanConvert(string dataTypeName);
        bool TryParse(object dataTypeValue, out object result);
    }
}