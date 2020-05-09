using System;

namespace CommandEngine.Models
{
    internal sealed class ParameterfulCommand : Command
    {
        internal Type CommandData { get; }

        internal Action<object> CommandAction { get; }

        internal CommandModelContext ModelContext { get; }

        internal ParameterfulCommand(Type commandData, Action<object> commandAction, CommandModelContext modelContext)
            : base("")
        {
            this.CommandData = commandData;
            this.CommandAction = commandAction;
            this.ModelContext = modelContext;
        }

        internal ParameterfulCommand(string helpText, Type commandData, Action<object> commandAction, CommandModelContext modelContext)
            : base(helpText)
        {
            this.CommandData = commandData;
            this.CommandAction = commandAction;
            this.ModelContext = modelContext;
        }
    }
}