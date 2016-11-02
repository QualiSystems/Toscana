using System.Collections.Generic;
using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal static class Bootstrapper
    {
        static Bootstrapper()
        {
            var container = new SimpleIocContainer();
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
            DependencyResolver.SetResolver(container);
        }

        internal static IToscaParser<ToscaServiceTemplate> GetToscaServiceTemplateParser()
        {
            return DependencyResolver.Current.GetService<IToscaParser<ToscaServiceTemplate>>();
        }

        internal static IToscaServiceTemplateLoader GetToscaServiceTemplateLoader()
        {
            return DependencyResolver.Current.GetService<IToscaServiceTemplateLoader>();
        }

        internal static IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return DependencyResolver.Current.GetService<IToscaCloudServiceArchiveLoader>();
        }

        internal static IToscaParser<ToscaMetadata> GetToscaMetadataParser()
        {
            return DependencyResolver.Current.GetService<IToscaParser<ToscaMetadata>>();
        }

        internal static ICloudServiceArchiveValidator GetToscaCloudServiceValidator()
        {
            return DependencyResolver.Current.GetService<ICloudServiceArchiveValidator>();
        }

        internal static IToscaCloudServiceArchiveSaver GetToscaCloudServiceArchiveSaver()
        {
            return DependencyResolver.Current.GetService<IToscaCloudServiceArchiveSaver>();
        }

        internal static IToscaDataTypeValueConverter GetParser(string type)
        {
<<<<<<< Updated upstream:Toscana/Engine/Bootstrapper.cs
            return DependencyResolver.Current.GetService<IToscaParserFactory>().GetParser(type);
=======
            return Current.GetService<IToscaDataTypeRegistry>().GetConverter(type);
>>>>>>> Stashed changes:Toscana/Engine/DependencyResolver.cs
        }

        internal static IToscaSerializer<ToscaServiceTemplate> GetToscaServiceTemplateSerializer()
        {
            return DependencyResolver.Current.GetService<IToscaSerializer<ToscaServiceTemplate>>();
        }

        internal static IToscaFileSystemSaver<ToscaServiceTemplate> GetToscaServiceTemplateSaver()
        {
            return DependencyResolver.Current.GetService<IToscaFileSystemSaver<ToscaServiceTemplate>>();
        }

        internal static IToscaPropertyMerger GetPropertyMerger()
        {
<<<<<<< Updated upstream:Toscana/Engine/Bootstrapper.cs
            return DependencyResolver.Current.GetService<IToscaPropertyMerger>();
=======
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
>>>>>>> Stashed changes:Toscana/Engine/DependencyResolver.cs
        }
   }
}