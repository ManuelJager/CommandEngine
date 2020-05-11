using CommandEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommandEngineTest
{
    [TestClass]
    public class ParserTest
    {
        private CommandEngine.Console testConsole;


        [TestInitialize]
        public void InitializeTest()
        {
            testConsole = new CommandEngine.Console();
            testConsole.Output += (output) => System.Console.WriteLine(output);
        }

        [TestCleanup]
        public void CleanUpTest()
        {
            testConsole = null;
        }
    }
}