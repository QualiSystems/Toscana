using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Toscana.Engine
{
    public interface IToscaServiceTemplateLoader
    {
        ToscaServiceTemplate Load(string filePath, string alternativePath = null);
        ToscaServiceTemplate Load(Stream stream, string alternativePath = null);
    }

    public class ToscaServiceTemplateLoader : IToscaServiceTemplateLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaParser<ToscaServiceTemplate> toscaParser;

        public ToscaServiceTemplateLoader(IFileSystem fileSystem,
            IToscaParser<ToscaServiceTemplate> toscaParser)
        {
            this.fileSystem = fileSystem;
            this.toscaParser = toscaParser;
        }

        public ToscaServiceTemplate Load(string filePath, string alternativePath)
        {
            var toscaSimpleProfileBuilder = new ToscaServiceTemplateBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, filePath, alternativePath);
            return toscaSimpleProfileBuilder.Build();
        }

        public ToscaServiceTemplate Load(Stream stream, string alternativePath = null)
        {
            var toscaSimpleProfileBuilder = new ToscaServiceTemplateBuilder();
            AppendFileToBuilder(toscaSimpleProfileBuilder, stream, alternativePath);
            return toscaSimpleProfileBuilder.Build();
        }

        private void AppendFileToBuilder(IToscaServiceTemplateBuilder toscaServiceTemplateBuilder, Stream stream, string alternativePath)
        {
            using (var streamReader = new StreamReader(stream))
            {
                ReadFromStreamReader(toscaServiceTemplateBuilder, streamReader, alternativePath);
            }
        }

        private void AppendFileToBuilder(IToscaServiceTemplateBuilder toscaServiceTemplateBuilder, string filePath, string alternativePath = null)
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
                ReadFromStreamReader(toscaServiceTemplateBuilder, streamReader, alternativePath);
            }
        }

        private void ReadFromStreamReader(IToscaServiceTemplateBuilder toscaServiceTemplateBuilder, StreamReader streamReader, string alternativePath)
        {
            var toscaSimpleProfile = toscaParser.Parse(streamReader.ReadToEnd());
            toscaServiceTemplateBuilder.Append(toscaSimpleProfile);
            foreach (var importFile in toscaSimpleProfile.Imports.SelectMany(import => import.Values))
            {
                AppendFileToBuilder(toscaServiceTemplateBuilder, importFile.File, alternativePath);
            }
        }
    }
}