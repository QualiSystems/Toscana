using System.Collections.Generic;
using System.IO.Abstractions;

namespace Toscana.Engine
{
    /// <summary>
    /// Registeres all the types
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {
        static DependencyResolver()
        {
            SetResolver(new DependencyResolver());
        }

        private static IDependencyResolver _current;
        private readonly SimpleIocContainer container;

        /// <summary>
        /// Returns the current Dependency Resolver
        /// </summary>
        public static IDependencyResolver Current
        {
            get { return _current; }
        }

        /// <summary>
        /// Sets the current Dependency Resolver
        /// </summary>
        /// <param name="resolver"></param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            _current = resolver;
        }

        internal DependencyResolver()
        {
            container = new SimpleIocContainer();
            container.Register<IFileSystem, FileSystem>();
            container.Register<IToscaValidator<ToscaMetadata>, ToscaValidator<ToscaMetadata>>();
            container.Register<IToscaValidator<ToscaServiceTemplate>, ToscaValidator<ToscaServiceTemplate>>();
            container
                .Register<IToscaValidator<ToscaCloudServiceArchive>, ToscaValidator<ToscaCloudServiceArchive>>();
            container.Register<IToscaDeserializer<ToscaServiceTemplate>, ToscaDeserializer<ToscaServiceTemplate>>
                ();
            container.Register<IToscaParser<ToscaServiceTemplate>, ToscaParser<ToscaServiceTemplate>>();
            container.Register<IToscaDeserializer<ToscaMetadata>, ToscaDeserializer<ToscaMetadata>>();
            container.Register<IToscaParser<ToscaMetadata>, ToscaParser<ToscaMetadata>>();
            container.Register<IToscaServiceTemplateLoader, ToscaServiceTemplateLoader>();
            container.Register<IToscaCloudServiceArchiveLoader, ToscaCloudServiceArchiveLoader>();
            container.Register<IToscaCloudServiceArchiveSaver, ToscaCloudServiceArchiveSaver>();
            container.Register<IToscaSerializer<ToscaMetadata>, ToscaSerializer<ToscaMetadata>>();
            container.Register<IToscaSerializer<ToscaServiceTemplate>, ToscaSerializer<ToscaServiceTemplate>>();
            container.Register<IToscaFileSystemSaver<ToscaServiceTemplate>, ToscaFileSystemSaver<ToscaServiceTemplate>>();
            container.Register<ITypeConvertersFactory, TypeConvertersFactory>();
            container.Register<IToscaPropertyMerger, ToscaPropertyMerger>();
            container.Register<IToscaPropertyCombiner, ToscaPropertyCombiner>();
            container.Register<ICloudServiceArchiveValidator, CloudServiceArchiveValidator>();
            container.RegisterSingleton<IToscaDataTypeRegistry>(
                () => new ToscaDataTypeRegistry(new List<IToscaDataTypeValueConverter>
                {
                    new ToscaBooleanDataTypeConverter(),
                    new ToscaStringDataTypeConverter(),
                    new ToscaIntegerDataTypeConverter(),
                    new ToscaFloatDataTypeConverter(),
                    new ToscaNullDataTypeConverter()
                }));
        }

        internal static IToscaParser<ToscaServiceTemplate> GetToscaServiceTemplateParser()
        {
            return Current.GetService<IToscaParser<ToscaServiceTemplate>>();
        }

        internal static IToscaServiceTemplateLoader GetToscaServiceTemplateLoader()
        {
            return Current.GetService<IToscaServiceTemplateLoader>();
        }

        internal static IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return Current.GetService<IToscaCloudServiceArchiveLoader>();
        }

        internal static IToscaParser<ToscaMetadata> GetToscaMetadataParser()
        {
            return Current.GetService<IToscaParser<ToscaMetadata>>();
        }

        internal static ICloudServiceArchiveValidator GetToscaCloudServiceValidator()
        {
            return Current.GetService<ICloudServiceArchiveValidator>();
        }

        internal static IToscaCloudServiceArchiveSaver GetToscaCloudServiceArchiveSaver()
        {
            return Current.GetService<IToscaCloudServiceArchiveSaver>();
        }

        internal static IToscaDataTypeValueConverter GetParser(string type)
        {
            return Current.GetService<IToscaDataTypeRegistry>().GetConverter(type);
        }

        internal static IToscaSerializer<ToscaServiceTemplate> GetToscaServiceTemplateSerializer()
        {
            return Current.GetService<IToscaSerializer<ToscaServiceTemplate>>();
        }

        internal static IToscaFileSystemSaver<ToscaServiceTemplate> GetToscaServiceTemplateSaver()
        {
            return Current.GetService<IToscaFileSystemSaver<ToscaServiceTemplate>>();
        }

        internal static IToscaPropertyMerger GetPropertyMerger()
        {
            return Current.GetService<IToscaPropertyMerger>();
        }

        /// <summary>
        /// Returns type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return container.GetService<T>();
        }

        /// <summary>
        /// Replaces registration with a new instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void Replace<T>(T instance)
        {
            container.RegisterSingleton(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTypeValueConverter"></param>
        public void RegisterDataTypeConverter(IToscaDataTypeValueConverter dataTypeValueConverter)
        {
            container.GetService<IToscaDataTypeRegistry>().Register(dataTypeValueConverter);
        }
    }
}