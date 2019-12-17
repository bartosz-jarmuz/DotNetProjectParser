using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetCore_Console_Tests
    {
      

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
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Console.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            project.PropertyGroups.Count.Should().Be(1);
            var debugProperties = project.PropertyGroups.Where(x => x.Condition.Configuration == "Debug").ToList();
            debugProperties.Count().Should().Be(1);

            var debugAny = debugProperties.Single(x => x.Condition.Platform == Platform.x64);
            debugAny.Should().NotBeNull();
            debugAny.Condition.Expression.Should().Be("'$(Configuration)|$(Platform)'=='Debug|x64'");
            debugAny.Condition.Platform.Should().Be(Platform.x64);

            debugAny.PlatformTarget.Should().Be(Platform.AnyCpu);
            debugAny.Optimize.Should().Be(true);
            debugAny.DefineConstants.Should().Be("DEBUG;TRACE");
            debugAny.WarningLevel.Should().Be(2);
            debugAny.AllowUnsafeBlocks.Should().Be(true);
            debugAny.TreatWarningsAsErrors.Should().Be(false);
            debugAny.WarningsAsErrors.Should().Be("CS1591");
            debugAny.Prefer32Bit.Should().Be(true);
            debugAny.OutputPath.Should().Be(@"out\debug");

            debugAny.AllProperties.Keys.Count.Should().Be(11);


        }

        [Test]
        public void TestLoad_Analyzers_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Console.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            var analyzers = project.Items.Where(x => x.ItemType == "PackageReference").ToList();

            analyzers.Select(x => x.ItemName).Should().Contain(new List<string>()
            {
                "Microsoft.CodeAnalysis.FxCopAnalyzers"
            });
        }
    }
}