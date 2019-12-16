using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DotNetProjectParser.Tests
{
    public static class TestUtils
    {
        public static DirectoryInfo GetTestsRoot()
        {
            var dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            var testsRoot = GoToParentByName(dir, "tests");
            ThrowIfNotExists(testsRoot.FullName);
            return testsRoot;
        }

        public static DirectoryInfo GoToParentByName(DirectoryInfo start, string parentName)
        {
            var parent = start.Parent;
            while (parent.Name != parentName)
            {
                parent = parent.Parent;
            }

            return parent;
        }

        public static FileInfo GetSampleProject(string name)
        {
            var testRoot = GetTestsRoot();

            var samplesFolder = new DirectoryInfo(Path.Combine(testRoot.FullName, "SampleProjects"));
            ThrowIfNotExists(samplesFolder.FullName);

            var projectFile = samplesFolder.EnumerateFiles("*.*", SearchOption.AllDirectories)
                .FirstOrDefault(x => x.Name == name);
            if (projectFile == null)
            {
                throw new FileNotFoundException($"Failed to find [{name}] anywhere under [{samplesFolder.FullName}]");
            }
            ThrowIfNotExists(projectFile);

            return projectFile;
        }

        private static void ThrowIfNotExists(FileInfo path)
        {
            if (!path.Exists)
            {
                throw new FileNotFoundException($"Failed to find a required test object: [{path}]");
            }
        }

        private static void ThrowIfNotExists(DirectoryInfo path)
        {
            if (!path.Exists)
            {
                throw new DirectoryNotFoundException($"Failed to find a required test object: [{path}]");
            }
        }

        private static void ThrowIfNotExists(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new InvalidOperationException($"Failed to find a required test object: [{path}]");
            }
        }
    }
}
