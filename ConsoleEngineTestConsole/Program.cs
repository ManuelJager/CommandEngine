using CommandEngine;
using CommandEngine.Models;
using System;
using static System.Console;

namespace CommandEngineTextConsole
{
    internal class Program
    {
        public class PrintCommand : ParameterfulCommand
        {
            [ArgumentDefinition(0, "Value to be printed")]
            public string value { get; set; } = "default";

            public override string Name => "print";

            public override void CommandAction()
            {
                WriteLine(value);
            }
        }

        private static string FormatOutput(string output)
        {
            return $"\n{output}\n";
        }

        private static void Main(string[] args)
        {
            var console = new CommandEngine.Console();

            console.Output += value => WriteLine(FormatOutput(value));

            console.Add(new PrintCommand());

            while (true)
            {
                try
                {
                    console.Parse(ReadLine());
                }
                catch (Exception e)
                {
                    WriteLine(FormatOutput(e.Message));
                }
            }
        }
    }
}