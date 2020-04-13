using System;
using CommandEngine.Models;
using CommandEngine.Exceptions;

namespace CommandEngine
{
    internal static class CommandModelBuilder
    {
        /// <summary>
        /// Build data object from the command parameters
        /// </summary>
        /// <param name="tokenizer"></param>
        internal static object Build(Tokenizer tokenizer, Type commandDataType)
        {
            var commandDataInstance = Activator.CreateInstance(commandDataType);

            SetDefaults(commandDataInstance, commandDataType);

            while (tokenizer.Token != Token.EOF)
            {
                switch (tokenizer.Token)
                {
                    case Token.Literal:
                        HandleLiteral(tokenizer, commandDataType, commandDataInstance);
                        break;
                    case Token.Flag:
                        HandleFlag(tokenizer, commandDataType, commandDataInstance);
                        break;
                    case Token.Key:
                        HandleKey(tokenizer, commandDataType, commandDataInstance);
                        break;
                    case Token.String:
                    case Token.Number:
                        throw new IncorrectCommandFormatException("A value type should be proceded by a key");
                }

                tokenizer.NextToken();
            }

            return commandDataInstance;
        }

        /// <summary>
        /// Sets default values of the instance
        /// </summary>
        private static void SetDefaults(object commandDataInstance, Type commandDataType)
        {
            foreach (var property in commandDataType.GetProperties())
            {
                // Set booleans to false
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(commandDataInstance, false, null);
                }
            }
        }

        /// <summary>
        /// Handle a leading literal token
        /// </summary>
        private static void HandleLiteral(Tokenizer tokenizer, Type commandDataType, object commandDataInstance)
        {
            throw new NotImplementedException("Literal arguments are not yet supported");
        }

        /// <summary>
        /// Handle a leading flag token
        /// </summary>
        private static void HandleFlag(Tokenizer tokenizer, Type commandDataType, object commandDataInstance)
        {
            var property = commandDataType.GetProperty(tokenizer.Value);

            property.SetValue(commandDataInstance, true, null);
        }

        /// <summary>
        /// Handle a leading key token
        /// </summary>
        private static void HandleKey(Tokenizer tokenizer, Type commandDataType, object commandDataInstance)
        {
            // Get property data
            var property = commandDataType.GetProperty(tokenizer.Value);
            var propertyType = property.PropertyType;

            // Skip to next token, expecting a value type
            tokenizer.NextToken();

            switch (tokenizer.Token)
            {
                case Token.Literal:
                    throw new IncorrectCommandFormatException("Cannot provide a literal value after a key");
                case Token.Flag:
                    throw new IncorrectCommandFormatException("Cannot provide a flag after a key");
                case Token.Key:
                    throw new IncorrectCommandFormatException("Cannot provide a key after a key");
                case Token.String:
                    if (propertyType == typeof(string))
                    {
                        property.SetValue(commandDataInstance, tokenizer.Value, null);
                    }
                    else
                    {
                        throw new IncorrectCommandFormatException($"Expected a number type, found {propertyType} instead");
                    }
                    break;
                case Token.Number:
                    if (propertyType == typeof(double))
                    {
                        property.SetValue(commandDataInstance, double.Parse(tokenizer.Value), null);
                    }
                    else if (propertyType == typeof(float))
                    {
                        property.SetValue(commandDataInstance, float.Parse(tokenizer.Value), null);
                    }
                    else if (propertyType == typeof(long))
                    {
                        property.SetValue(commandDataInstance, long.Parse(tokenizer.Value), null);
                    }
                    else if (propertyType == typeof(int))
                    {
                        property.SetValue(commandDataInstance, int.Parse(tokenizer.Value), null);
                    }
                    else
                    {
                        throw new IncorrectCommandFormatException($"Expected a number type, found {propertyType} instead");
                    }
                    break;
                case Token.EOF:
                    throw new IncorrectCommandFormatException("Expected a value after a key");
            }
        }
    }
}
