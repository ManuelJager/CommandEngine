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
        public Action<object> ParameterfulAction { get; }
        public Action ParameterlessAction { get; }
        public CommandModelContext ModelContext { get; }
        public bool IsParameterfulCommand { get; }

        public CommandContainer(Type commandData, Action<object> commandAction, CommandModelContext modelContext)
        {
            this.CommandData = commandData;
            this.ParameterfulAction = commandAction;
            this.ParameterlessAction = null;
            this.ModelContext = modelContext;
            this.IsParameterfulCommand = true;
        }

        public CommandContainer(Action commandAction)
        {
            this.CommandData = null;
            this.ParameterfulAction = null;
            this.ParameterlessAction = commandAction;
            this.ModelContext = null;
            this.IsParameterfulCommand = true;
        }
    }
}
