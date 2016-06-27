using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Toscana.Engine
{
    public interface IToscaSimpleProfileLoader
    {
        ToscaSimpleProfile Load(string filePath);
        ToscaSimpleProfile Load(Stream stream);
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

        public ToscaSimpleProfile Load(string filePath)
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, filePath);
            return toscaSimpleProfileBuilder.Build();
        }

        public ToscaSimpleProfile Load(Stream stream)
        {
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, stream);
            return toscaSimpleProfileBuilder.Build();
        }

        private void AppendFileToBuilder(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                ReadFromStreamReader(toscaSimpleProfileBuilder, streamReader);
            }
        }

        private void AppendFileToBuilder(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, string filePath)
        {
            using (var streamReader = fileSystem.File.OpenText(filePath))
            {
                ReadFromStreamReader(toscaSimpleProfileBuilder, streamReader);
            }
        }

        private void ReadFromStreamReader(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder,
            StreamReader streamReader)
        {
            var toscaSimpleProfile = toscaSimpleProfileParser.Parse(streamReader.ReadToEnd());
            toscaSimpleProfileBuilder.Append(toscaSimpleProfile);
            foreach (var importFile in toscaSimpleProfile.Imports.SelectMany(import => import.Values))
            {
                AppendFileToBuilder(toscaSimpleProfileBuilder, importFile.File);
            }
        }
    }
}