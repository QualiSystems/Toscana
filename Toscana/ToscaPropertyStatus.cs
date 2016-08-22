namespace Toscana
{
    /// <summary>
    /// Defines status of property valid values
    /// </summary>
    public enum ToscaPropertyStatus
    {
        /// <summary>
        /// Indicates the property is supported.  This is the default value for all property definitions.
        /// </summary>
        supported,


        /// <summary>
        /// Indicates the property is not supported.
        /// </summary>
        unsupported,


        /// <summary>
        /// Indicates the property is experimental and has no official standing.
        /// </summary>
        experimental,


        /// <summary>
        /// Indicates the property has been deprecated by a new specification version.
        /// </summary>
        deprecated
    }
}