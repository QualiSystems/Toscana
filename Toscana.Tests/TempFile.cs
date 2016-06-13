using System;
using System.IO;

namespace Toscana.Tests
{
    public class TempFile : IDisposable
    {
        private readonly string filePath;

        public TempFile(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);
            filePath = Path.Combine(baseDirectory, Guid.NewGuid().ToString());
        }

        public string FilePath
        {
            get { return filePath; }
        }

        public void Dispose()
        {
            if (File.Exists(filePath))
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                {
                    
                }
        }
    }
}