using CommandEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommandEngine
{
    sealed public partial class Parser
    {
        private Dictionary<string, Command> commandCollection = new Dictionary<string, Command>();
        private ILogger HelpLogger { get; }

        public Parser(ILogger helpLogger)
        {
            this.HelpLogger = helpLogger;
            WithCommandTokenizer("help", HelpAction);
        }

        /// <summary>
        /// Add a command that exposes the input reader
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Parser WithCommandTokenizer(string alias, Action<Tokenizer> action)
        {
            return WithCommandTokenizer(alias, "", action);
        }

        /// <summary>
        /// Add a command that exposes the input reader
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Parser WithCommandTokenizer(string alias, string helpText, Action<Tokenizer> action)
        {
            commandCollection.Add(alias, new ExposedCommand(action));

            return this;
        }

        /// <summary>
        /// Add command to the parser
        /// </summary>
        /// <typeparam name="ParamT">Type of the command data model</typeparam>
        /// <exception cref="IncorrectModelFormatException">Signals incorrect parameter object format</exception>
        /// <param name="alias">name of the command</param>
        /// <param name="action">command handler</param>
        /// <returns></returns>
        public Parser WithCommand<ParamT>(string alias, Action<ParamT> action)
        {
            return WithCommand<ParamT>(alias, "", action);
        }

        /// <summary>
        /// Add command to the parser
        /// </summary>
        /// <typeparam name="ParamT">Type of the command data model</typeparam>
        /// <exception cref="IncorrectModelFormatException">Signals incorrect parameter object format</exception>
        /// <param name="alias">name of the command</param>
        /// <param name="action">command handler</param>
        /// <returns></returns>
        public Parser WithCommand<ParamT>(string alias, string helpText, Action<ParamT> action)
        {
            // Validate data model
            var typeContext = ValidateAndBuildContext(typeof(ParamT));

            // Because the object builder cannot know the type,
            // we must wrap the action by another one that converts the object to the correct type for the action to be called
            Action<object> invoker = (data) =>
            {
                action((ParamT)data);
            };

            commandCollection.Add(alias, new ParameterfulCommand(helpText, typeof(ParamT), invoker, typeContext));

            return this;
        }

        /// <summary>
        /// Add a parameterless command
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Parser WithCommand(string alias, Action action)
        {
            return WithCommand(alias, "", action);
        }

        /// <summary>
        /// Add a parameterless command
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Parser WithCommand(string alias, string helpText, Action action)
        {
            commandCollection.Add(alias, new ParameterlessCommand(action));

            return this;
        }

        /// <summary>
        /// Parse string input and build parameter object if applicable
        /// </summary>
        /// <exception cref="IncorrectCommandFormatException">Signifies incorrect command format</exception>
        /// <param name="input"></param>
        public void Parse(string input)
        {
            var tokenizer = new Tokenizer(new StringReader(input));

            // Get first word
            var commandAlias = tokenizer.Value;

            // Must be a word
            if (tokenizer.Token != Token.Literal)
            {
                throw new IncorrectCommandFormatException("First input must be an argument");
            }

            // Command alias must exist
            if (!commandCollection.ContainsKey(commandAlias))
            {
                throw new IncorrectCommandFormatException("First input must a registered command");
            }

            // Get command info
            var commandBase = commandCollection[commandAlias];

            tokenizer.NextToken();

            // If the command has parameters
            if (commandBase.GetType() == typeof(ParameterfulCommand))
            {
                var command = (ParameterfulCommand)commandBase;

                // model used as parameter for the command invoker
                var dataInstance = CommandModelBuilder.Build(tokenizer, command.CommandData, command.ModelContext);

                // Invoke the command action with the built model
                command.CommandAction(dataInstance);
            }
            else if (commandBase.GetType() == typeof(ParameterlessCommand))
            {
                var command = (ParameterlessCommand)commandBase;

                // In case of a parameterless command, the user must not input any arguments
                if (tokenizer.Token != Token.EOF)
                {
                    throw new IncorrectCommandFormatException($"Command {commandAlias} is parameterless");
                }
                else
                {
                    command.CommandAction();
                }
            }
            else if (commandBase.GetType() == typeof(ExposedCommand))
            {
                var command = (ExposedCommand)commandBase;

                command.CommandAction(tokenizer);
            }
        }

        /// <summary>
        /// Validate commmandData layout
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static CommandModelContext ValidateAndBuildContext(Type commandData)
        {
            var orders = new List<int>();
            var properties = commandData.GetProperties();
            var aliases = new List<string>();
            var modelContext = new CommandModelContext();

            foreach (var property in properties)
            {
                try
                {
                    var argument = property.GetProperty<ArgumentDefinitionAttribute>();

                    // Handle property types
                    if (!(
                        property.PropertyType.IsEnum ||
                        property.PropertyType == typeof(string) ||
                        property.PropertyType == typeof(double) ||
                        property.PropertyType == typeof(float) ||
                        property.PropertyType == typeof(long) ||
                        property.PropertyType == typeof(bool) ||
                        property.PropertyType == typeof(int)))
                    {
                        throw new IncorrectModelFormatException($"Unsupported type of {property.PropertyType} present on property {property.Name}");
                    }

                    // Index properties to the model context
                    if (argument.Positional)
                    {
                        var order = argument.ArgumentOrder;
                        orders.Add(order);
                        modelContext.positionalProperties[order] = property;
                        modelContext.propertiesHelpText[property.Name] = argument.HelpText;
                    }
                    else
                    {
                        if (argument.aliases == null)
                        {
                            var alias = property.Name;
                            aliases.Add(alias);
                            modelContext.aliasedProperties[alias] = property;
                            modelContext.propertiesHelpText[alias] = argument.HelpText;
                        }
                        else
                        {
                            for (int i = 0; i < argument.aliases.Length; i++)
                            {
                                var alias = argument.aliases[i];
                                aliases.Add(alias);
                                modelContext.aliasedProperties[alias] = property;
                                modelContext.propertiesHelpText[alias] = argument.HelpText;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IncorrectModelFormatException($"Missing argument definition attribute on property {property.Name}");
                }
            };

            // Handle positional arguments
            if (orders.Count != 0)
            {
                var sortedOrders = orders.ToArray();
                Array.Sort(sortedOrders);

                if (sortedOrders[0] != 0)
                {
                    throw new IncorrectModelFormatException($"Order index of the first argument is 0 based");
                }

                if (!AreElementsContiguous(sortedOrders))
                {
                    throw new IncorrectModelFormatException($"Found a incorrect argument value order");
                }
            }

            var duplicateAliases = aliases.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            if (duplicateAliases.Count() > 0)
            {
                throw new IncorrectModelFormatException($"Duplicate aliases found, aliases are ${string.Join(", ", duplicateAliases)}");
            }

            return modelContext;
        }

        private static bool AreElementsContiguous(int[] array)
        {
            // After sorting, check if current element is one more
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] - array[i - 1] != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}