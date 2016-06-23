using System.IO.Abstractions;

namespace Toscana.Engine
{
    public static class Bootstrapper
    {
        public static IToscaSimpleProfileParser GetToscaSimpleProfileParser()
        {
            return new ToscaSimpleProfileParser(new ToscaValidator(), new ToscaDeserializer());
        }

        public static ToscaSimpleProfileLoader GetToscaSimpleProfileLoader(
            IToscaSimpleProfileParser toscaSimpleProfileParser = null, IFileSystem fileSystem = null)
        {
            fileSystem = fileSystem ?? new FileSystem();
            toscaSimpleProfileParser = toscaSimpleProfileParser ?? GetToscaSimpleProfileParser();
            return new ToscaSimpleProfileLoader(fileSystem,toscaSimpleProfileParser);
        }
    }
}