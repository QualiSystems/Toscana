using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Toscana.Engine
{
    public interface IToscaSimpleProfileLoader
    {
        ToscaSimpleProfile Load(string filePath, string alternativePath = null);
        ToscaSimpleProfile Load(Stream stream, string alternativePath = null);
    }

    public class ToscaSimpleProfileLoader : IToscaSimpleProfileLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaSimpleProfileParser toscaSimpleProfileParser;

        public ToscaSimpleProfileLoader(IFileSystem fileSystem,
            IToscaSimpleProfileParser toscaSimpleProfileParser)
        {
            this.fileSystem = fileSystem;
            this.toscaSimpleProfileParser = toscaSimpleProfileParser;
        }

        public ToscaSimpleProfile Load(string filePath, string alternativePath)
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, filePath, alternativePath);
            return toscaSimpleProfileBuilder.Build();
        }

        public ToscaSimpleProfile Load(Stream stream, string alternativePath = null)
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, stream, alternativePath);
            return toscaSimpleProfileBuilder.Build();
        }

        private void AppendFileToBuilder(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, Stream stream, string alternativePath)
        {
            using (var streamReader = new StreamReader(stream))
            {
                ReadFromStreamReader(toscaSimpleProfileBuilder, streamReader, alternativePath);
            }
        }

        private void AppendFileToBuilder(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, string filePath, string alternativePath = null)
        {
            if (!fileSystem.File.Exists(filePath) && !string.IsNullOrEmpty(alternativePath))
            {
                var alternativeFullPath = fileSystem.Path.Combine(alternativePath, filePath);
                if (fileSystem.File.Exists(alternativeFullPath))
                {
                    filePath = alternativeFullPath;
                }
            }
            using (var streamReader = fileSystem.File.OpenText(filePath))
            {
                ReadFromStreamReader(toscaSimpleProfileBuilder, streamReader, alternativePath);
            }
        }

        private void ReadFromStreamReader(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, StreamReader streamReader, string alternativePath)
        {
            var toscaSimpleProfile = toscaSimpleProfileParser.Parse(streamReader.ReadToEnd());
            toscaSimpleProfileBuilder.Append(toscaSimpleProfile);
            foreach (var importFile in toscaSimpleProfile.Imports.SelectMany(import => import.Values))
            {
                AppendFileToBuilder(toscaSimpleProfileBuilder, importFile.File, alternativePath);
            }
        }
    }
}