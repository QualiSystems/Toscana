using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using FluentAssertions;
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
                return GetYamlFilesContentFromUrl(repositoryUrl, IsYaml);
            }

            public static List<FileContent> GetYamlFilesContentFromUrl(string repositoryUrl, Func<ZipArchiveEntry, bool> filesFilter)
            {
                using (var tempFile = new TempFile(Path.GetTempPath()))
                using (var client = new WebClient())
                {
                    client.DownloadFile(repositoryUrl, tempFile.FilePath);

                    using (var zipToOpen = new FileStream(tempFile.FilePath, FileMode.Open))
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        return archive.Entries
                            .Where(filesFilter)
                            .Select(ReadFileContent)
                            .ToList();
                    }
                }
            }

            public static bool IsYaml(ZipArchiveEntry a)
            {
                return Path.GetExtension(a.Name).EqualsAny(".yaml", ".yml");
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
            string toscaAsString = fileContent.Content;
            using (var memoryStream = toscaAsString.ToMemoryStream())
            {
                ToscaServiceTemplate.Load(memoryStream);
            }
        }

        [Test, TestCaseSource(typeof(LocalDirectoryTestCasesFactory), "TestCases")]
        [Ignore]
        public void Validate_Tosca_Files_In_Local_Directory(FileContent fileContent)
        {
            using (var memoryStream = fileContent.Content.ToMemoryStream())
            {
                ToscaServiceTemplate.Load(memoryStream);
            }
        }

        [Test]
        public void Build_Combined_Tosca_Service_Template_From_Github_Repo()
        {
            var filesContent = GithubRepositoryTestCasesFactory.GetYamlFilesContentFromUrl(GithubRepositoryZip, GithubRepositoryTestCasesFactory.IsYaml);
            var toscaSimpleProfileBuilder = new ToscaServiceTemplateBuilder();
            foreach (var fileContent in filesContent)
            {
                using (var memoryStream = fileContent.Content.ToMemoryStream())
                {
                    toscaSimpleProfileBuilder.Append(memoryStream);
                }
            }
            toscaSimpleProfileBuilder.Build();
        }

        [Test]
        [Ignore]
        public void Load_Tosca_Cloud_Service_Archive_From_Github()
        {
            using (var tempFile = new TempFile(Path.GetTempPath()))
            using (var client = new WebClient())
            {
                client.DownloadFile(GithubRepositoryZip, tempFile.FilePath);

                var toscaCloudServiceArchive = ToscaCloudServiceArchive.Load(tempFile.FilePath);

                var entryLeafNodeTypes = toscaCloudServiceArchive.GetEntryLeafNodeTypes();
                entryLeafNodeTypes.Should().HaveCount(1);
                var nxosNodeType = entryLeafNodeTypes.Single().Value;
                nxosNodeType.Properties.Should().ContainKey("device_owner");
            }
        }
    }
}