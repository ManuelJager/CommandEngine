using System;

namespace CommandEngine.Models
{
    /// <summary>
    /// Command instance.
    /// Has no state
    /// </summary>
    internal sealed class CommandContainer
    {
        public Type CommandData { get; }

        // Either an action<object> or action
        public dynamic CommandAction { get; }

        public CommandModelContext ModelContext { get; }

        public bool IsParameterfulCommand { get; }

        public CommandContainer(Type commandData, Action<object> commandAction, CommandModelContext modelContext)
        {
            this.CommandData = commandData;
            this.CommandAction = commandAction;
            this.ModelContext = modelContext;
            this.IsParameterfulCommand = true;
        }

        public CommandContainer(Action commandAction)
        {
            this.CommandData = null;
            this.CommandAction = commandAction;
            this.ModelContext = null;
            this.IsParameterfulCommand = false;
        }
    }
}