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

        public CommandContainer(Type commandData, Action<object> commandAction)
        {
            this.commandData = commandData;
            this.commandAction = commandAction;
        }
    }
}
