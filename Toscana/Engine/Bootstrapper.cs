using System.Collections.Generic;
using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal static class Bootstrapper
    {
        internal static SimpleIocContainer RegisterTypes()
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
            container.Register<IToscaParserFactory>(
                () => new ToscaParserFactory(new List<IToscaDataTypeValueConverter>
                {
                    new ToscaBooleanDataTypeConverter(),
                    new ToscaStringDataTypeConverter(),
                    new ToscaIntegerDataTypeConverter(),
                    new ToscaFloatDataTypeConverter(),
                    new ToscaNullDataTypeConverter()
                }));
            return container;
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
            return DependencyResolver.Current.GetService<IToscaParserFactory>().GetParser(type);
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
            return DependencyResolver.Current.GetService<IToscaPropertyMerger>();
        }
   }
}