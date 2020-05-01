using System;

namespace CommandEngine.Models
{
    /// <summary>
    /// Command instance.
    /// Has no state
    /// </summary>
    internal sealed class CommandContainer
    {
        public Type commandData { get; }
        public Action<object> commandAction { get; }
        public CommandModelContext modelContext { get; }

        public CommandContainer(Type commandData, Action<object> commandAction, CommandModelContext modelContext)
        {
            this.commandData = commandData;
            this.commandAction = commandAction;
            this.modelContext = modelContext;
        }
    }
}
