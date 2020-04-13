using System;

namespace CommandEngine.Exceptions
{
    sealed class IncorrectCommandFormatException : Exception
    {
        public IncorrectCommandFormatException(string message)
            : base(message)
        {
        }
    }
}
