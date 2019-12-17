// -----------------------------------------------------------------------
//  <copyright file="NetFrameworkProject_Console_ParserTests.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetFramework_Console_Tests
    {
        [Test]
        public void TestLoad_StaticProperties_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Console.csproj");

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

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = "Program.cs",
                ItemName = "Program.cs",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Program.cs"),
                ItemType = "Compile",
                CopyToOutputDirectory = null,
                Project = project
            });

            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                Include = @"SomeXmlFile.xml",
                ItemName = "SomeXmlFile.xml",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "SomeXmlFile.xml"),
                ItemType = "None",
                CopyToOutputDirectory = "Always",
                Project = project
            });

            var compileItems = project.GetCompileItems();
            Assert.AreEqual(2, compileItems.Count());
            compileItems.Select(x => x.ItemName).Should().Contain(new[] { "Program.cs", "AssemblyInfo.cs" });
        }


        [Test]
        public void TestLoad_PropertyGroups_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Console.csproj");

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
            debugAny.TreatWarningsAsErrors.Should().Be(false);
            debugAny.WarningsAsErrors.Should().Be("CS1591");


            debugAny.AllProperties.Keys.Count.Should().Be(10);


            var releaseX64 = project.PropertyGroups.Single(x =>
                x.Condition.Platform == Platform.x64 && x.Condition.Configuration == "Release");
            releaseX64.Should().NotBeNull();
            releaseX64.Condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Release|x64'");
            releaseX64.Condition.Platform.Should().Be(Platform.x64);

            releaseX64.PlatformTarget.Should().Be(Platform.x64);
            releaseX64.OutputPath.Should().Be(@"bin\x64\Release\");
            releaseX64.Optimize.Should().Be(true);
            releaseX64.DefineConstants.Should().Be("TRACE");
            releaseX64.WarningLevel.Should().Be(4);
            releaseX64.AllowUnsafeBlocks.Should().Be(true);
            releaseX64.TreatWarningsAsErrors.Should().Be(false);
            releaseX64.Prefer32Bit.Should().Be(true);
            releaseX64.WarningsAsErrors.Should().Be("");

            releaseX64.AllProperties.Keys.Count.Should().Be(10);
        }
    }
}