using System.IO.Abstractions;
using System.Linq;

namespace Toscana.Engine
{
    public class ToscaSimpleProfileLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaSimpleProfileParser toscaSimpleProfileParser;

        public ToscaSimpleProfileLoader(IFileSystem fileSystem)
            : this(fileSystem, Bootstrapper.GetToscaSimpleProfileParser())
        {
            this.fileSystem = fileSystem;
        }

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

        private void AppendFileToBuilder(IToscaSimpleProfileBuilder toscaSimpleProfileBuilder, string filePath)
        {
            using (var streamReader = fileSystem.File.OpenText(filePath))
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
}