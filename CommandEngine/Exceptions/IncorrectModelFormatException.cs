using System;

namespace CommandEngine.Exceptions
{
    sealed class IncorrectModelFormatException : Exception
    {
        public IncorrectModelFormatException(string message)
            : base(message)
        {
        }
    }
}
