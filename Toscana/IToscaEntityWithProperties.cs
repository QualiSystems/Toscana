using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// Defines an interface with Properties
    /// </summary>
    public interface IToscaEntityWithProperties<T> : IDerivableToscaEntity<T> where T : IDerivableToscaEntity<T>
    {
        /// <summary>
        /// An optional list of property definitions for the Node Type.
        /// </summary>
        Dictionary<string, ToscaProperty> Properties { get; set; }
    }
}