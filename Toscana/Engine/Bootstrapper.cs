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
            poorManContainer.Register<IToscaSimpleProfileDeserializer, ToscaSimpleProfileDeserializer>();
            poorManContainer.Register<IToscaMetadataDeserializer, ToscaMetadataDeserializer>();
            poorManContainer.Register<IToscaSimpleProfileParser, ToscaSimpleProfileParser>();
            poorManContainer.Register<IToscaSimpleProfileLoader, ToscaSimpleProfileLoader>();
            poorManContainer.Register<IToscaCloudServiceArchiveLoader, ToscaCloudServiceArchiveLoader>();
        }

        public IToscaSimpleProfileParser GetToscaSimpleProfileParser()
        {
            return poorManContainer.GetInstance<IToscaSimpleProfileParser>();
        }

        public IToscaSimpleProfileLoader GetToscaSimpleProfileLoader()
        {
            return poorManContainer.GetInstance<IToscaSimpleProfileLoader>();
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