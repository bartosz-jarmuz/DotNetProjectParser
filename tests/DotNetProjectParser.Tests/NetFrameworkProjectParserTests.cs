using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetFrameworkProjectParserTests
    {
        [Test]
        public void TestProjectLoad_Console_PropertiesReadOk()
        {
            var fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Console.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            project.ProjectXml.Should().NotBeNull();
            project.Name.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Console.csproj");
            project.FullPath.Should().Be(fileInfo.FullName);
            project.DirectoryPath.Should().Be(fileInfo.Directory.FullName);
            project.AssemblyName.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Console");
            project.OutputType.Should().Be("Exe");
            project.TargetExtension.Should().Be(".exe");
            project.TargetFramework.Should().Be("v4.7.2");
            //project.TreatWarningsAsErrors.Should().Be(false);
            //project.WarningsAsErrors.Should().Be("CS1591");

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = "Program.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Program.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"SomeXmlFile.xml",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "SomeXmlFile.xml"),
                ItemType = "None",
                CopyToOutputDirectory = "Always",
                Project = project
            });

            project.PropertyGroups.Count.Should().Be(6);
            var debugProperties = project.PropertyGroups.Where(x => x.Condition.Configuration == "Debug").ToList();
            debugProperties.Count().Should().Be(2);

            var debugAny = debugProperties.Single(x => x.Condition.Platform == Platform.AnyCpu);
            debugAny.Should().NotBeNull();
            debugAny.Condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'");
            debugAny.Condition.Platform.Should().Be(Platform.AnyCpu);

            debugAny.PlatformTarget.Should().Be(Platform.AnyCpu);
            debugAny.DebugSymbols.Should().Be(true);
            debugAny.DebugType.Should().Be("full");
            debugAny.Optimize.Should().Be(false);
            debugAny.DefineConstants.Should().Be("DEBUG;TRACE");
            debugAny.ErrorReport.Should().Be("prompt");
            debugAny.WarningLevel.Should().Be(4);
            debugAny.AllowUnsafeBlocks.Should().Be(false);
            debugAny.TreatWarningsAsErrors.Should().Be(false);
            debugAny.WarningsAsErrors.Should().Be("CS1591");


            debugAny.AllProperties.Keys.Count.Should().Be(10);
        }


        [Test]
        public void TestProjectLoad_Library_PropertiesReadOk()
        {
            var fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Library.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            project.ProjectXml.Should().NotBeNull();
            project.Name.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Library.csproj");
            project.FullPath.Should().Be(fileInfo.FullName);
            project.DirectoryPath.Should().Be(fileInfo.Directory.FullName);
            project.AssemblyName.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Library");
            project.OutputType.Should().Be("Library");
            project.TargetExtension.Should().Be(".dll");
            project.TargetFramework.Should().Be("v4.7");
          

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = "Class1.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Class1.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.PropertyGroups.Count.Should().Be(6);
            var debugProperties  = project.PropertyGroups.Where(x => x.Condition.Configuration == "Debug").ToList();
            debugProperties.Count().Should().Be(2);

            var debugAny = debugProperties.Single(x => x.Condition.Platform == Platform.AnyCpu);
            debugAny.Should().NotBeNull();
            debugAny.Condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'");
            debugAny.Condition.Platform.Should().Be(Platform.AnyCpu);

            debugAny.PlatformTarget.Should().Be(Platform.AnyCpu);
            debugAny.DebugSymbols.Should().Be(true);
            debugAny.DebugType.Should().Be("full");
            debugAny.Optimize.Should().Be(false);
            debugAny.DefineConstants.Should().Be("DEBUG;TRACE");
            debugAny.ErrorReport.Should().Be("prompt");
            debugAny.WarningLevel.Should().Be(4);
            debugAny.AllowUnsafeBlocks.Should().Be(false);
            debugAny.TreatWarningsAsErrors.Should().Be(true);
            debugAny.WarningsAsErrors.Should().Be("");


            debugAny.AllProperties.Keys.Count.Should().Be(10);

        }

    }
}