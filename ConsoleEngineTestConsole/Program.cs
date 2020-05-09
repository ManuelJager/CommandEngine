using CommandEngine;
using System;
using System.Threading;

namespace CommandEngineTextConsole
{
    internal class Program
    {
        public class PrintArgs
        {
            [ArgumentDefinition(0, "Value to be printed")]
            public string Value { get; set; }
        }

        private static string FormatOutput(string output)
        {
            return $"\n{output}\n";
        }

        private static void Main(string[] args)
        {
            var parser = new Parser();

            parser.Output += (output) => Console.WriteLine(FormatOutput(output));

            parser.WithCommand("q", "quits to desktop", () =>
            {
                Console.WriteLine("Quitting...");
                Thread.Sleep(1000);
                Environment.Exit(0);
            });

            parser.WithCommand<PrintArgs>("echo", "prints a message to the console", (data) =>
            {
                Console.WriteLine(data.Value);
            });

            while (true)
            {
                Console.Write("Input a command > ");
                try
                {
                    parser.Parse(Console.ReadLine());
                }
                catch (IncorrectCommandFormatException e)
                {
                    Console.WriteLine(FormatOutput(e.Message));
                }
            }
        }
    }
}