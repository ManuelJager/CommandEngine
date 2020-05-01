using System;

namespace CommandEngine.Exceptions
{
    internal sealed class IncorrectCommandFormatException : Exception
    {
        public IncorrectCommandFormatException(string message)
            : base(message)
        {
        }
    }
}