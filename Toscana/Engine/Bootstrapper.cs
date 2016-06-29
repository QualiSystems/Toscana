using System.IO.Abstractions;

namespace Toscana.Engine
{
    public class Bootstrapper
    {
        private readonly PoorManContainer poorManContainer;

        public Bootstrapper()
        {
            poorManContainer = new PoorManContainer();
            poorManContainer.Register<IFileSystem, FileSystem>();
            poorManContainer.Register<IToscaValidator, ToscaValidator>();
            poorManContainer.Register<IToscaServiceTemplateDeserializer, ToscaServiceTemplateDeserializer>();
            poorManContainer.Register<IToscaMetadataDeserializer, ToscaMetadataDeserializer>();
            poorManContainer.Register<IToscaServiceTemplateParser, ToscaServiceTemplateParser>();
            poorManContainer.Register<IToscaServiceTemplateLoader, ToscaServiceTemplateLoader>();
            poorManContainer.Register<IToscaCloudServiceArchiveLoader, ToscaCloudServiceArchiveLoader>();
        }

        public IToscaServiceTemplateParser GetToscaSimpleProfileParser()
        {
            return poorManContainer.GetInstance<IToscaServiceTemplateParser>();
        }

        public IToscaServiceTemplateLoader GetToscaSimpleProfileLoader()
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
    }
}