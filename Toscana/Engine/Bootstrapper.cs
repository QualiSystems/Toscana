namespace Toscana.Engine
{
    public static class Bootstrapper
    {
        public static ToscaSimpleProfileParser GetToscaSimpleProfileParser()
        {
            return new ToscaSimpleProfileParser(new ToscaValidator(), new ToscaDeserializer());
        }
    }
}