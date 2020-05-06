using System;

namespace CommandEngine
{
    public sealed class IncorrectModelFormatException : Exception
    {
        public IncorrectModelFormatException(string message)
            : base(message)
        {
        }
    }
}