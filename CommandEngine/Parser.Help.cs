using CommandEngine.Models;
using System.Collections.Generic;

namespace CommandEngine
{
    sealed public partial class Parser
    {
        private void HelpAction(Tokenizer tokenizer)
        {
            // Handle no parameter
            if (tokenizer.Token == Token.EOF)
            {
                Output.Invoke("Availible commands: \n\n" + string.Join("\n", commandCollection.Keys));
            }
            // handle first parameter
            else if (tokenizer.Token == Token.Literal)
            {
                HelpHandleFirstParameter(tokenizer);
            }
            // handle invalid parameter
            else throw new IncorrectCommandFormatException("Expected a command name");
        }

        private void HelpHandleFirstParameter(Tokenizer tokenizer)
        {
            var commandName = tokenizer.Value;
            if (!commandCollection.ContainsKey(commandName))
            {
                // handle non existant command
                throw new IncorrectCommandFormatException($"Command \"{commandName}\" doesn't exist");
            }

            // Get command
            var arg0 = tokenizer.Value;
            var command = commandCollection[arg0];

            // Go to second parameter
            tokenizer.NextToken();

            if (tokenizer.Token == Token.EOF)
            {
                HelpHandleCommandOutput(command, commandName);
            }
            else if (tokenizer.Token == Token.Literal)
            {
                HelpHandleSecondParameter(tokenizer, command);
            }
            // handle incorrect command property format
            else throw new IncorrectCommandFormatException("Expected a command property");
        }

        private void HelpHandleCommandOutput(Command command, string commandName)
        {
            var lines = new List<string>
            {
                $"{commandName}: {command.HelpText}"
            };

            // if the command contains properties, print them out
            if (command.GetType() == typeof(ParameterfulCommand))
            {
                var propertiesHelpText = ((ParameterfulCommand)command).ModelContext.propertiesHelpText;

                lines.Add("Properties: ");

                foreach (var keyValuePair in propertiesHelpText)
                {
                    lines.Add($"\t{keyValuePair.Key}: {keyValuePair.Value}");
                }
            }

            Output.Invoke(string.Join("\n", lines));
        }

        private void HelpHandleSecondParameter(Tokenizer tokenizer, Command command)
        {
            if (command.GetType() == typeof(ParameterfulCommand))
            {
                var propertiesHelpText = ((ParameterfulCommand)command).ModelContext.propertiesHelpText;
                var arg1 = tokenizer.Value;

                if (propertiesHelpText.ContainsKey(arg1))
                {
                    var helpText = propertiesHelpText[arg1];
                    // log command property
                    Output.Invoke($"{arg1}: {helpText}");
                }
                // handle property not found
                else throw new IncorrectCommandFormatException($"Command doesn't contain property \"{arg1}\"");
            }
            // handle incorrect command type
            else throw new IncorrectCommandFormatException("Command contains no defined properties");
        }
    }
}