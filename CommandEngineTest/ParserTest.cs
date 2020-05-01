using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandEngine;

namespace CommandEngineTest
{
    [TestClass]
    public class ParserTest
    {
        class TestArguments
        {
            [ArgumentDefinition(HelpText: "test help text")]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseString()
        {
            var value = "default";
            Parser.Instance.WithCommand<TestArguments>("test", (data) =>
            {
                value = data.text;
            })
            .Parse("test --text \"value\"");
            Assert.AreEqual("value", value);
        }

        class TestArgumentsAliased
        {
            [ArgumentDefinition(HelpText: "test help text", "aliased-text", "aliased-text-two")]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseStringAliased()
        {
            var value = "default";
            Parser.Instance.WithCommand<TestArgumentsAliased>("test", (data) =>
            {
                value = data.text;
            });
            Parser.Instance.Parse("test --aliased-text \"value\"");
            Assert.AreEqual("value", value);
            Parser.Instance.Parse("test --aliased-text-two \"valuu\"");
            Assert.AreEqual("valuu", value);
        }

        class TestArgumentsIndexed
        {
            [ArgumentDefinition(HelpText: "test help text", 0)]
            public string text { get; set; }
        }

        [TestMethod]
        public void TestParseStringIndexed()
        {
            var value = "default";
            Parser.Instance.WithCommand<TestArgumentsIndexed>("test", (data) =>
            {
                value = data.text;
            })
            .Parse("test \"value\"");
            Assert.AreEqual("value", value);
        }


        [TestCleanup]
        public void CleanUpTest()
        {
            Parser.Instance = null;
        }
    }
}
