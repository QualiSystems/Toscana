using System.IO;
using System.IO.Abstractions;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    internal interface IToscaServiceTemplateLoader
    {
        /// <summary>
        /// Loads ToscaServiceTemplate from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ToscaParsingException">Thrown when file is not valid according to YAML or TOSCA</exception>
        ToscaServiceTemplate Load(string filePath);
    }

    internal class ToscaServiceTemplateLoader : IToscaServiceTemplateLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaParser<ToscaServiceTemplate> toscaParser;

        public ToscaServiceTemplateLoader(IFileSystem fileSystem,
            IToscaParser<ToscaServiceTemplate> toscaParser)
        {
            this.fileSystem = fileSystem;
            this.toscaParser = toscaParser;
        }

        /// <summary>
        /// Loads ToscaServiceTemplate from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ToscaParsingException">Thrown when file is not valid according to YAML or TOSCA</exception>
        public ToscaServiceTemplate Load(string filePath)
        {
            using (var stream = fileSystem.File.Open(filePath, FileMode.Open))
            {
                return toscaParser.Parse(stream);
            }
        }
    }
}