using System.IO;
using DotNetProjectParser.ProjectModel;
using FluentAssertions;
using NUnit.Framework;

namespace DotNetProjectParser.Tests
{

    [TestFixture]
    public class NetCoreProjectParserTests
    {
     
        [Test]
        public void TestProjectLoad_PropertiesReadOk()
        {
            var fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Console.csproj");

            var fileFactory = new ProjectFileFactory();
            var project = fileFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            project.ProjectXml.Should().NotBeNull();
            project.Name.Should().Be("DotNetProjectParser.SampleProjects.NetCore.Console.csproj");
            project.FullPath.Should().Be(fileInfo.FullName);
            project.DirectoryPath.Should().Be(fileInfo.Directory.FullName);
            project.AssemblyName.Should().Be("DotNetProjectParser.SampleProjects.NetCore.Console");
            project.OutputType.Should().Be("Exe");
            project.TargetExtension.Should().Be(".exe");
            project.TargetFramework.Should().Be("netcoreapp2.2");


            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"SomeXmlFile.xml",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "SomeXmlFile.xml"),
                ItemType = "None",
                CopyToOutputDirectory = "Always",
                Project = project

            });
        }
    }
}