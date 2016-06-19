using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Toscana.Common;
using Toscana.Domain;

namespace Toscana.Tests
{
    public class GithubRepositoryTests
    {
        public const string GithubRepositoryZip = "https://github.com/qualisystems/tosca/archive/develop.zip";

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
                return GetYamlFilesContentFromUrl(repositoryUrl);
            }

            public static IEnumerable<string> GetYamlFilesContentFromUrl(string repositoryUrl)
            {
                using (var tempFile = new TempFile(Path.GetTempPath()))
                using (var client = new WebClient())
                {
                    client.DownloadFile(repositoryUrl, tempFile.FilePath);

                    var zipToOpen = new FileStream(tempFile.FilePath, FileMode.Open);
                    var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);

                    return archive.Entries.Where(a =>
                        Path.GetExtension(a.Name).EqualsAny(".yaml", ".yml"))
                        .Select(ReadFileContent)
                        .ToList();
                }
            }

            private static string ReadFileContent(ZipArchiveEntry archiveEntry)
            {
                using (var streamReader = new StreamReader(archiveEntry.Open()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        [Test, TestCaseSource(typeof (GithubRepositoryTestCasesFactory), "TestCases")]
        public void Validate_Tosca_Files_In_Github_Repository_Of_Quali(string toscaFileContent)
        {
            ToscaSimpleProfile.Parse(toscaFileContent);
        }

        [Test]
        public void Build_Combined_Tosca_Simple_Profile_From_Github_Repo()
        {
            var filesContent = GithubRepositoryTestCasesFactory.GetYamlFilesContentFromUrl(GithubRepositoryZip);
            var toscaSimpleProfiles = filesContent.Select(ToscaSimpleProfile.Parse).ToArray();
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            foreach (var toscaSimpleProfile in toscaSimpleProfiles)
            {
                toscaSimpleProfileBuilder.Append(toscaSimpleProfile);
            }
            toscaSimpleProfileBuilder.Build();
        }
    }
}