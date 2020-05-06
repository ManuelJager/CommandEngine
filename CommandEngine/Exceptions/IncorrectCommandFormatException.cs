using System;

namespace CommandEngine
{
    public sealed class IncorrectCommandFormatException : Exception
    {
        public IncorrectCommandFormatException(string message)
            : base(message)
        {
        }
    }
}