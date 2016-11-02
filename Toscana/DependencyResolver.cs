using Toscana.Engine;

namespace Toscana
{
    /// <summary>
    /// Common interface for esolving dependencies
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Resolves a service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>();

        /// <summary>
        /// Replaces registration with a new instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        void Replace<T>(T instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTypeValueConverter"></param>
        void RegisterDataTypeConverter(IToscaDataTypeValueConverter dataTypeValueConverter);
    }
}