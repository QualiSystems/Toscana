using System.Collections.Generic;
using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal class Bootstrapper
    {
        public static Bootstrapper Current = new Bootstrapper();

        private readonly SimpleIocContainer container;

        internal Bootstrapper()
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
            container.Register<IToscaServiceTemplateSaver, ToscaServiceTemplateSaver>();
            container.Register<ITypeConvertersFactory, TypeConvertersFactory>();
            container.Register<IToscaPropertyMerger, ToscaPropertyMerger>();
            container.Register<IToscaPropertyCombiner, ToscaPropertyCombiner>();
            container.Register<IToscaParserFactory>(
                () => new ToscaParserFactory(new List<IToscaDataTypeValueConverter>
                {
                    new ToscaBooleanDataTypeConverter(),
                    new ToscaStringDataTypeConverter(),
                    new ToscaIntegerDataTypeConverter(),
                    new ToscaFloatDataTypeConverter(),
                    new ToscaNullDataTypeConverter()
                }));
        }

        internal IToscaParser<ToscaServiceTemplate> GetToscaServiceTemplateParser()
        {
            return container.GetInstance<IToscaParser<ToscaServiceTemplate>>();
        }

        internal IToscaServiceTemplateLoader GetToscaServiceTemplateLoader()
        {
            return container.GetInstance<IToscaServiceTemplateLoader>();
        }

        internal IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return container.GetInstance<IToscaCloudServiceArchiveLoader>();
        }

        public Bootstrapper Replace<T>(T instance)
        {
            container.RegisterSingleton(instance);
            return this;
        }

        internal IToscaParser<ToscaMetadata> GetToscaMetadataParser()
        {
            return container.GetInstance<IToscaParser<ToscaMetadata>>();
        }

        internal IToscaValidator<ToscaCloudServiceArchive> GetToscaCloudServiceValidator()
        {
            return container.GetInstance<IToscaValidator<ToscaCloudServiceArchive>>();
        }

        internal IToscaCloudServiceArchiveSaver GetToscaCloudServiceArchiveSaver()
        {
            return container.GetInstance<IToscaCloudServiceArchiveSaver>();
        }

        internal IToscaDataTypeValueConverter GetParser<T>(string type)
        {
            var toscaParserFactory = container.GetInstance<IToscaParserFactory>();
            return toscaParserFactory.GetParser(type);
        }

        internal IToscaSerializer<ToscaServiceTemplate> GetToscaServiceTemplateSerializer()
        {
            return container.GetInstance<IToscaSerializer<ToscaServiceTemplate>>();
        }

        internal IToscaServiceTemplateSaver GetToscaServiceTemplateSaver()
        {
            return container.GetInstance<IToscaServiceTemplateSaver>();
        }

        internal IToscaPropertyMerger GetPropertyMerger()
        {
            return container.GetInstance<IToscaPropertyMerger>();
        }
    }
}