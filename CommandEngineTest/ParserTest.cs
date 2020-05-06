using CommandEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommandEngineTest
{
    [TestClass]
    public class ParserTest
    {
        public class TestLogger : ILogger
        {
            public void Log(string value)
            {
                Console.WriteLine(value);
            }
        }

        private Parser testParser;

        private class TestArguments
        {
            [ArgumentDefinition(HelpText: "test help text")]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseString()
        {
            var value = "default";
            testParser.WithCommand<TestArguments>("test", (data) =>
            {
                value = data.text;
            });
            testParser.Parse("test --text \"value\"");
            Assert.AreEqual("value", value);
        }

        private class TestArgumentsAliased
        {
            [ArgumentDefinition(aliases: new string[] { "aliased-text", "aliased-text-two" }, HelpText: "test help text")]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseStringAliased()
        {
            var value = "default";
            testParser.WithCommand<TestArgumentsAliased>("test", (data) =>
            {
                value = data.text;
            });
            testParser.Parse("test --aliased-text \"value\"");
            Assert.AreEqual("value", value);
            testParser.Parse("test --aliased-text-two \"valuu\"");
            Assert.AreEqual("valuu", value);
        }

        private class TestArgumentsIndexed
        {
            [ArgumentDefinition(ArgumentOrder: 0, HelpText: "test help text")]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseStringIndexed()
        {
            var value = "default";
            testParser.WithCommand<TestArgumentsIndexed>("test", (data) =>
            {
                value = data.text;
            })
            .Parse("test \"value\"");
            Assert.AreEqual("value", value);
        }

        private class EnumTestArguments
        {
            public enum gitMode { pull, push }

            [ArgumentDefinition(ArgumentOrder: 0)]
            public gitMode Mode { get; set; }
        }

        [TestMethod]
        public void TestParseEnum()
        {
            var value = "default";
            testParser.WithCommand<EnumTestArguments>("git", (data) =>
            {
                switch (data.Mode)
                {
                    case EnumTestArguments.gitMode.pull:
                        value = "pulled";
                        break;

                    case EnumTestArguments.gitMode.push:
                        value = "pushed";
                        break;
                }
            });
            testParser.Parse("git push"); // user inpt
            Assert.AreEqual("pushed", value);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            testParser = new Parser(new TestLogger());
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            testParser = null;
        }
    }
}