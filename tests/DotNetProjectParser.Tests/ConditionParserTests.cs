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
        public void TestProjectLoad_AllMethodsReturnEquivalentResult()
        {
            XAttribute attribute = new XAttribute("Condition", " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");

            var condition = ConditionParser.Parse(attribute);
            condition.Expression.Should().Be("'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'");
            condition.Platform.Should().Be(Platform.AnyCpu);
            condition.Configuration.Should().Be("Debug");

        }
    }
}