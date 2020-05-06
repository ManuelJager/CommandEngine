using System.Collections.Generic;
using System.Reflection;

namespace CommandEngine.Models
{
    internal class CommandModelContext
    {
        public CommandModelContext()
        {
            this.aliasedProperties = new Dictionary<string, PropertyInfo>();
            this.positionalProperties = new Dictionary<int, PropertyInfo>();
            this.propertiesHelpText = new Dictionary<string, string>();
        }

        // links aliases to the property info of the type
        internal Dictionary<string, PropertyInfo> aliasedProperties { get; }

        // links positional index to the property info of the type
        internal Dictionary<int, PropertyInfo> positionalProperties { get; }

        // links property to its help text
        internal Dictionary<string, string> propertiesHelpText { get; }
    }
}