namespace Toscana.Tests
{
    public class FileContent
    {
        private readonly string filename;
        private readonly string content;

        public FileContent(string filename, string content)
        {
            this.filename = filename;
            this.content = content;
        }

        public string Filename
        {
            get { return filename; }
        }

        public string Content
        {
            get { return content; }
        }

        public override string ToString()
        {
            return filename;
        }
    }
}