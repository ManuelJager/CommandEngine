using System;

namespace CommandEngine.Models
{
    internal sealed class ExposedCommand : Command
    {
        public Action<Tokenizer> CommandAction { get; }

        public ExposedCommand(Action<Tokenizer> commandAction)
            : base("")
        {
            this.CommandAction = commandAction;
        }

        public ExposedCommand(string helpText, Action<Tokenizer> commandAction)
            : base(helpText)
        {
            this.CommandAction = commandAction;
        }
    }
}