using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal class Bootstrapper
    {
        private readonly PoorManContainer poorManContainer;

        public Bootstrapper()
        {
            poorManContainer = new PoorManContainer();
            poorManContainer.Register<IFileSystem, FileSystem>();
            poorManContainer.Register<IToscaValidator<ToscaMetadata>, ToscaValidator<ToscaMetadata>>();
            poorManContainer.Register<IToscaValidator<ToscaServiceTemplate>, ToscaValidator<ToscaServiceTemplate>>();
            poorManContainer.Register<IToscaValidator<ToscaCloudServiceArchive>, ToscaValidator<ToscaCloudServiceArchive>>();
            poorManContainer.Register<IToscaDeserializer<ToscaServiceTemplate>, ToscaDeserializer<ToscaServiceTemplate>>();
            poorManContainer.Register<IToscaParser<ToscaServiceTemplate>, ToscaParser<ToscaServiceTemplate>>();
            poorManContainer.Register<IToscaDeserializer<ToscaMetadata>, ToscaDeserializer<ToscaMetadata>>();
            poorManContainer.Register<IToscaParser<ToscaMetadata>, ToscaParser<ToscaMetadata>>();
            poorManContainer.Register<IToscaServiceTemplateLoader, ToscaServiceTemplateLoader>();
            poorManContainer.Register<IToscaCloudServiceArchiveLoader, ToscaCloudServiceArchiveLoader>();
            poorManContainer.Register<IToscaCloudServiceArchiveSaver, ToscaCloudServiceArchiveSaver>();
            poorManContainer.Register<IToscaSerializer<ToscaMetadata>, ToscaSerializer<ToscaMetadata>>();
            poorManContainer.Register<IToscaSerializer<ToscaServiceTemplate>, ToscaSerializer<ToscaServiceTemplate>>();
            poorManContainer.Register<ITypeConvertersFactory, TypeConvertersFactory>();
        }

        public IToscaParser<ToscaServiceTemplate> GetToscaServiceTemplateParser()
        {
            return poorManContainer.GetInstance<IToscaParser<ToscaServiceTemplate>>();
        }

        public IToscaServiceTemplateLoader GetToscaServiceTemplateLoader()
        {
            return poorManContainer.GetInstance<IToscaServiceTemplateLoader>();
        }

        public IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return poorManContainer.GetInstance<IToscaCloudServiceArchiveLoader>();
        }

        public Bootstrapper Replace<T>(T instance)
        {
            poorManContainer.RegisterSingleton(instance);
            return this;
        }

        public IToscaParser<ToscaMetadata> GetToscaMetadataParser()
        {
            return poorManContainer.GetInstance<IToscaParser<ToscaMetadata>>();
        }

        public IToscaValidator<ToscaCloudServiceArchive> GetToscaCloudServiceValidator()
        {
            return poorManContainer.GetInstance<IToscaValidator<ToscaCloudServiceArchive>>();
        }

        public IToscaCloudServiceArchiveSaver GetToscaCloudServiceArchiveSaver()
        {
            return poorManContainer.GetInstance<IToscaCloudServiceArchiveSaver>();
        }
    }
}