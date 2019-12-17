using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetCore_Library_Tests
    {

        [Test]
        public void TestLoad_Items_ReadOk()
        {
            var fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Library.csproj");

            Project project = ProjectFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            Assert.IsFalse(project.Items.Any(x=>x.ItemName == "I should not be visible.txt"));

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = "Class1.cs",
                ItemName = "Class1.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Class1.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"Subfolder\File2.cs",
                ItemName = @"File2.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Subfolder\\File2.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"File3.cs",
                ItemName = @"File3.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "File3.cs"),
                ItemType = "Resource",
                CopyToOutputDirectory = "PreserveNewest",
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @".startWithDot",
                ItemName = @".startWithDot",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, ".startWithDot"),
                ItemType = "None",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"NoneDoNotCopy.bmp",
                ItemName = @"NoneDoNotCopy.bmp",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "NoneDoNotCopy.bmp"),
                ItemType = "None",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"ResourceCopyAlways.bmp",
                ItemName = @"ResourceCopyAlways.bmp",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "ResourceCopyAlways.bmp"),
                ItemType = "Resource",
                CopyToOutputDirectory = "Always",
                Project = project
            });


            var compileItems = project.GetCompileItems();
            Assert.AreEqual(2, compileItems.Count());
            compileItems.Select(x => x.ItemName).Should().Contain(new[] {"Class1.cs", "File2.cs"});
        }

        [Test]
        public void TestLoad_StaticProperties_ReadOk()
        {
            var fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Library.csproj");

            var project = ProjectFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            project.ProjectXml.Should().NotBeNull();
            project.Name.Should().Be("DotNetProjectParser.SampleProjects.NetCore.Library.csproj");
            project.FullPath.Should().Be(fileInfo.FullName);
            project.DirectoryPath.Should().Be(fileInfo.Directory.FullName);
            project.AssemblyName.Should().Be("DotNetProjectParser.SampleProjects.NetCore.Library");
            project.OutputType.Should().Be("Library");
            project.TargetExtension.Should().Be(".dll");
            project.TargetFramework.Should().Be("netcoreapp3.0");

        }

        [Test]
        public void TestLoad_PropertyGroups_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Library.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            project.PropertyGroups.Count.Should().Be(0);

        }
    }
}