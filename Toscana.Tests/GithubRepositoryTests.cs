using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Toscana.Common;

namespace Toscana.Tests
{
    public class GithubRepositoryTests
    {
        public const string GithubRepositoryZip = "https://github.com/qualisystems/tosca/archive/master.zip";

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

            public static List<FileContent> GetYamlFilesContentFromUrl(string repositoryUrl)
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

            private static FileContent ReadFileContent(ZipArchiveEntry archiveEntry)
            {
                using (var streamReader = new StreamReader(archiveEntry.Open()))
                {
                    return new FileContent(archiveEntry.Name, streamReader.ReadToEnd());
                }
            }
        }

        private class LocalDirectoryTestCasesFactory
        {
            public static IEnumerable TestCases
            {
                get
                {
                    return GetYamlFilesFromLocalDirectory(@"C:\work\github\tosca");
                }
            }

            private static IEnumerable GetYamlFilesFromLocalDirectory(string directoryPath)
            {
                return GetYamlFilesContentFromDirectory(directoryPath);
            }

            public static List<TestCaseData> GetYamlFilesContentFromDirectory(string directoryPath)
            {
                return Directory.EnumerateFiles(directoryPath).Where(a =>
                    Path.GetExtension(a).EqualsAny(".yaml", ".yml"))
                    .Select(ReadFileContent)
                    .Select(_=>new TestCaseData(_))
                    .ToList();
            }

            private static FileContent ReadFileContent(string filePath)
            {
                using (var streamReader = File.OpenText(filePath))
                {
                    return new FileContent(filePath, streamReader.ReadToEnd());
                }
            }
        }

        [Test, TestCaseSource(typeof (GithubRepositoryTestCasesFactory), "TestCases")]
        public void Validate_Tosca_Files_In_Github_Repository_Of_Quali(FileContent fileContent)
        {
            ToscaSimpleProfile.Parse(fileContent.Content);
        }

        [Test, TestCaseSource(typeof(LocalDirectoryTestCasesFactory), "TestCases")]
        public void Validate_Tosca_Files_In_Local_Directory(FileContent fileContent)
        {
            ToscaSimpleProfile.Parse(fileContent.Content);
        }

        [Test]
        public void Build_Combined_Tosca_Simple_Profile_From_Github_Repo()
        {
            var filesContent = GithubRepositoryTestCasesFactory.GetYamlFilesContentFromUrl(GithubRepositoryZip);
            var toscaSimpleProfileBuilder = new ToscaSimpleProfileBuilder();
            foreach (var fileContent in filesContent)
            {
                toscaSimpleProfileBuilder.Append(fileContent.Content);
            }
            toscaSimpleProfileBuilder.Build();
        }
    }
}