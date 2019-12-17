// -----------------------------------------------------------------------
//  <copyright file="InstantiationTests.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Xml.Linq;
using DotNetProjectParser.Readers;
using FluentAssertions;
using NUnit.Framework;

namespace DotNetProjectParser.Tests
{
    [TestFixture]
    public class ConditionParserTests
    {

        [Test]
        public void TestParse_NetFramework_ResultOk()
        {
            XAttribute attribute = new XAttribute("Condition", " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");

            var condition = ConditionParser.Parse(attribute);
            condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'");
            condition.Platform.Should().Be(Platform.AnyCpu);
            condition.Configuration.Should().Be("Debug");

        } 
        [Test]
        public void TestParse_NetCore_ResultOk()
        {
            XAttribute attribute = new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Debug|x64'");

            var condition = ConditionParser.Parse(attribute);
            condition.Expression.Should().Be("'$(Configuration)|$(Platform)'=='Debug|x64'");
            condition.Platform.Should().Be(Platform.x64);
            condition.Configuration.Should().Be("Debug");

        }
    }
}