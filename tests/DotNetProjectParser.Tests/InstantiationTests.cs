// -----------------------------------------------------------------------
//  <copyright file="InstantiationTests.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class InstantiationTests
    {

        [Test]
        public void TestProjectLoad_AllMethodsReturnEquivalentResult()
        {
            FileInfo fileInfo = TestUtils.GetSampleProject(@"DotNetProjectParser.SampleProjects.NetCore.Console.csproj");

            Project projectFromStaticFactory = ProjectFactory.GetProject(fileInfo);
            Project projectFromConstructMethod = Project.Construct(fileInfo);
            IProjectFactory factory = new ProjectFactory();
            Project projectFromInstanceFactory = factory.GetProject(fileInfo);

            projectFromStaticFactory.Should().BeEquivalentTo(projectFromConstructMethod, options => options.IgnoringCyclicReferences());
            projectFromStaticFactory.Should().BeEquivalentTo(projectFromInstanceFactory, options => options.IgnoringCyclicReferences());

        }
    }
}