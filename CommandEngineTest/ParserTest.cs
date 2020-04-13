using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandEngine;

namespace CommandEngineTest
{
    [TestClass]
    public class ParserTest
    {
        class TestArguments
        {
            [ArgumentDefinition(Required: true, HelpText: "test help text")]
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
    }
}
