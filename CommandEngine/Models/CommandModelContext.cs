using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandEngine.Models
{
    class CommandModelContext
    {
        public CommandModelContext()
        {
            this.aliasedProperties = new Dictionary<string, PropertyInfo>();
            this.positionalProperties = new Dictionary<int, PropertyInfo>();
        }

        // links aliases to the property info of the type
        public Dictionary<string, PropertyInfo> aliasedProperties { get; }

        // links and positional index to the property info of the type
        public Dictionary<int, PropertyInfo> positionalProperties { get; }
    }
}
