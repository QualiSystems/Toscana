using System.IO.Abstractions;

namespace Toscana.Engine
{
    public static class Bootstrapper
    {
        public static IToscaSimpleProfileParser GetToscaSimpleProfileParser()
        {
            return new ToscaSimpleProfileParser(new ToscaValidator(), new ToscaSimpleProfileDeserializer());
        }

        public static ToscaSimpleProfileLoader GetToscaSimpleProfileLoader(
            IToscaSimpleProfileParser toscaSimpleProfileParser = null, IFileSystem fileSystem = null)
        {
            fileSystem = fileSystem ?? new FileSystem();
            toscaSimpleProfileParser = toscaSimpleProfileParser ?? GetToscaSimpleProfileParser();
            return new ToscaSimpleProfileLoader(fileSystem,toscaSimpleProfileParser);
        }

        public static ToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return new ToscaCloudServiceArchiveLoader(new FileSystem());
        }
    }
}