using System;

namespace CommandEngine.Exceptions
{
    internal sealed class IncorrectModelFormatException : Exception
    {
        public IncorrectModelFormatException(string message)
            : base(message)
        {
        }
    }
}