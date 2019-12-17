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
    public class TreatAllWarningsAsErrorsTests
    {
        [Test]
        public void NetFramework_TreatAllWarningsAsErrorsTests_IsTrue()
        {
            var fileInfo =
                TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetFramework.WarningsAsErrors.csproj");

            var project = DotNetProjectParser.ProjectFactory.GetProject(fileInfo);
            project.IsTreatingWarningsAsErrorsInAllConfigurations().Should().Be(true);

        }


    }
}