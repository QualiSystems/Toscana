namespace Toscana.Engine
{
    /// <summary>
    /// Represents an interface for parsing custom data types
    /// </summary>
    public interface IToscaDataTypeValueConverter
    {
        /// <summary>
        /// Determines whether can parse data type
        /// </summary>
        /// <param name="dataTypeName"></param>
        /// <returns></returns>
        bool CanConvert(string dataTypeName);


        /// <summary>
        /// Tries to parse and returns whether it succeeded or not
        /// </summary>
        /// <param name="dataTypeValue">Raw value to parse</param>
        /// <param name="result">Parsed value</param>
        /// <returns>True if succeeded to parser, False otherwise</returns>
        bool TryParse(object dataTypeValue, out object result);
    }
}