using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class NetFramework_Database_Tests
    {
        [Test]
        public void TestLoad_StaticProperties_ReadOk()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.Db.sqlproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);

            project.Should().NotBeNull();
            project.ProjectXml.Should().NotBeNull();
            project.Name.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Db.sqlproj");
            project.FullPath.Should().Be(fileInfo.FullName);
            project.DirectoryPath.Should().Be(fileInfo.Directory.FullName);
            project.AssemblyName.Should().Be("DotNetProjectParser.SampleProjects.NetFramework.Db");
            project.OutputType.Should().Be("Database");
            project.TargetExtension.Should().Be(".dll");
            project.TargetFramework.Should().Be("v4.7.2");


            project.Items.Should().ContainEquivalentOf(new ProjectItem()
            {
                ItemName = "Function1.sql",
                Include = "Functions\\Function1.sql",
                ResolvedIncludePath = Path.Combine(fileInfo.Directory.FullName, "Functions", "Function1.sql"),
                ItemType = "Build",
                CopyToOutputDirectory = null,
                Project = project
            });

            Assert.AreEqual(1, project.Items.Count(x=>x.ItemName == "Script1.sql"));
            Assert.AreEqual(1, project.Items.Count(x=>x.ItemName == "Procedure1.sql"));



        }

        
    }
}