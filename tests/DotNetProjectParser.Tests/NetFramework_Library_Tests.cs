using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetFramework_Library_Tests
    {
        [Test]
        public void TestLoad_StaticProperties_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Library.csproj");

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
                ItemName = "Class1.cs",
                Include = "Class1.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Class1.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });
        }

        [Test]
        public void TestLoad_PropertyGroups_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Library.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);


            project.PropertyGroups.Count.Should().Be(6);
            var debugProperties = project.PropertyGroups.Where(x => x.Condition.Configuration == "Debug").ToList();
            debugProperties.Count().Should().Be(2);

            var debugAny = debugProperties.Single(x => x.Condition.Platform == Platform.AnyCpu);
            debugAny.Should().NotBeNull();
            debugAny.Condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'");
            debugAny.Condition.Platform.Should().Be(Platform.AnyCpu);

            debugAny.PlatformTarget.Should().Be(Platform.AnyCpu);
            debugAny.Optimize.Should().Be(false);
            debugAny.DefineConstants.Should().Be("DEBUG;TRACE");
            debugAny.WarningLevel.Should().Be(4);
            debugAny.AllowUnsafeBlocks.Should().Be(false);
            debugAny.TreatWarningsAsErrors.Should().Be(true);
            debugAny.WarningsAsErrors.Should().Be("");


            debugAny.AllProperties.Keys.Count.Should().Be(10);


        }

        [Test]
        public void TestLoad_Analyzers_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Library.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            var analyzers = project.Items.Where(x => x.ItemType == "Analyzer").ToList();

            analyzers.Select(x => x.ItemName).Should().Contain(new List<string>()
            {
                "Microsoft.CodeAnalysis.VersionCheckAnalyzer.dll" ,
                "Microsoft.CodeQuality.Analyzers.dll" ,
                "Microsoft.CodeQuality.CSharp.Analyzers.dll" ,
                "Microsoft.NetCore.Analyzers.dll" ,
                "Microsoft.NetCore.CSharp.Analyzers.dll" ,
                "Microsoft.NetFramework.Analyzers.dll" ,
                "Microsoft.NetFramework.CSharp.Analyzers.dll"
            });
        }
    }
}