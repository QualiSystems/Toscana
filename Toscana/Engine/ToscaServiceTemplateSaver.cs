using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal interface IToscaServiceTemplateSaver
    {
        void Save(string path, ToscaServiceTemplate toscaServiceTemplate);
    }

    internal class ToscaServiceTemplateSaver : IToscaServiceTemplateSaver
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaSerializer<ToscaServiceTemplate> serializer;

        public ToscaServiceTemplateSaver(IFileSystem fileSystem, IToscaSerializer<ToscaServiceTemplate> serializer)
        {
            this.fileSystem = fileSystem;
            this.serializer = serializer;
        }

        public void Save(string path, ToscaServiceTemplate toscaServiceTemplate)
        {
            using (var stream = fileSystem.File.Create(path))
            {
                serializer.Serialize(stream, toscaServiceTemplate);
            }
        }
    }
}