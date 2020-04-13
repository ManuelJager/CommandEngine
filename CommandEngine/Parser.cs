using System;
using System.Collections.Generic;
using System.IO;
using CommandEngine.Exceptions;
using CommandEngine.Models;

namespace CommandEngine
{
    sealed public class Parser
    {
        private static Parser instance = null;

        public static Parser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Parser();
                }
                return instance;
            }
        }

        private Dictionary<string, CommandContainer> commandCollection = new Dictionary<string, CommandContainer>();

        /// <summary>
        /// Add command the parser
        /// </summary>
        /// <typeparam name="ParamT">Type of the command data model</typeparam>
        /// <param name="alias">name of the command</param>
        /// <param name="action">command handler</param>
        /// <returns></returns>
        public Parser WithCommand<ParamT>(string alias, Action<ParamT> action)
        {
            // Validate data model
            try
            {
                Validate(typeof(ParamT));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            // Because the object builder cannot know the type, 
            // we must wrap the action by another one that converst the object to the correct type for the action to be called
            Action<object> invoker = (data) =>
            {
                action((ParamT)data);
            };

            commandCollection.Add(alias, new CommandContainer(typeof(ParamT), invoker));

            return this;
        }

        public void Parse(string input)
        {
            var tokenizer = new Tokenizer(new StringReader(input));

            try
            {
                if (tokenizer.Token != Token.Literal)
                {
                    throw new IncorrectCommandFormatException("First input must be an argument");
                }

                if (!commandCollection.ContainsKey(tokenizer.Value))
                {
                    throw new IncorrectCommandFormatException("First input must a registered command");
                }

                var container = commandCollection[tokenizer.Value];

                tokenizer.NextToken();

                // model used as parameter for the command invoker
                var dataInstance = CommandModelBuilder.Build(tokenizer, container.commandData);

                // Invoke the command action with the built model
                container.commandAction(dataInstance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Validate commmandData layout
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static bool Validate(Type commandData)
        {
            var orders = new List<int>();

            var properties = commandData.GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var argument = property.GetProperty<ArgumentDefinitionAttribute>();

                    if (!(
                        property.PropertyType == typeof(string) ||
                        property.PropertyType == typeof(double) ||
                        property.PropertyType == typeof(float) ||
                        property.PropertyType == typeof(long) ||
                        property.PropertyType == typeof(bool) ||
                        property.PropertyType == typeof(int)))
                    {
                        throw new IncorrectModelFormatException($"Unsupported type of {property.PropertyType} present on property {property.Name}");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IncorrectModelFormatException($"Missing argument definition attribute on property {property.Name}");
                }
            };

            return true;
        }
    }
}
