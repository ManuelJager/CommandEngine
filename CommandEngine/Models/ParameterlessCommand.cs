using System;

namespace CommandEngine.Models
{
    internal sealed class ParameterlessCommand : Command
    {
        public Action CommandAction { get; }

        internal ParameterlessCommand(Action commandAction)
            : base("")
        {
            this.CommandAction = commandAction;
        }

        internal ParameterlessCommand(string helpText, Action commandAction)
            : base(helpText)
        {
            this.CommandAction = commandAction;
        }
    }
}