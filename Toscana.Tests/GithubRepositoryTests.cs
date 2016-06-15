using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Engine;

namespace Toscana.Tests
{
    public class GithubRepositoryTests
    {
        private const string GithubRepositoryZip = "https://github.com/qualisystems/tosca/archive/master.zip";

        private class GithubRepositoryTestCasesFactory
        {
            public static IEnumerable TestCases
            {
                get
                {
                    return GetYamlFilesFromZippedUrl(GithubRepositoryZip);
                }
            }

            private static IEnumerable GetYamlFilesFromZippedUrl(string repositoryUrl)
            {
                using (var tempFile = new TempFile(Path.GetTempPath()))
                using (var client = new WebClient())
                {
                    client.DownloadFile(repositoryUrl, tempFile.FilePath);

                    var zipToOpen = new FileStream(tempFile.FilePath, FileMode.Open);
                    var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

                    return archive.Entries.Where(a =>
                        Path.GetExtension(a.Name).EqualsAny(".yaml", ".yml"))
                        .Select(archiveEntry => new TestCaseData(archiveEntry)).ToList();
                }
            }
        }

        [Test, TestCaseSource(typeof (GithubRepositoryTestCasesFactory), "TestCases")]
        public void Validate_Tosca_Files_In_Github_Repository_Of_Quali(ZipArchiveEntry zipArchiveEntry)
        {
            var toscaNetAnalyzer = new ToscaNetAnalyzer();

            toscaNetAnalyzer.Analyze(new StreamReader(zipArchiveEntry.Open()));
        }
    }
}