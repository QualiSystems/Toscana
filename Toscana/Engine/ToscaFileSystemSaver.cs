using System.IO.Abstractions;

namespace Toscana.Engine
{
    internal interface IToscaFileSystemSaver<T>
    {
        void Save(string path, T toscaServiceTemplate);
    }

    internal class ToscaFileSystemSaver<T> : IToscaFileSystemSaver<T>
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaSerializer<T> serializer;

        public ToscaFileSystemSaver(IFileSystem fileSystem, IToscaSerializer<T> serializer)
        {
            this.fileSystem = fileSystem;
            this.serializer = serializer;
        }

        public void Save(string path, T toscaObject)
        {
            using (var stream = fileSystem.File.Create(path))
            {
                serializer.Serialize(stream, toscaObject);
            }
        }
    }
}