using System;

namespace CommandEngine
{
    /// <summary>
    /// Provides context for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ArgumentDefinitionAttribute : Attribute
    {
        /// <summary>
        /// Argument tooltip
        /// </summary>
        public string HelpText { get; }

        /// <summary>
        /// Order in the argument list
        /// </summary>
        public int ArgumentOrder { get; }

        public string[] aliases { get; }

        /// <summary>
        /// Flags if the argument key should be typed out in full, or if the value should be inferred from the position of itself in the argument list
        /// </summary>
        public bool Positional => ArgumentOrder != -1;

        public ArgumentDefinitionAttribute(string HelpText = "default help text")
        {
            this.HelpText = HelpText;
            this.ArgumentOrder = -1;
            this.aliases = null;
        }

        public ArgumentDefinitionAttribute(int ArgumentOrder = -1, string HelpText = "default help text")
        {
            this.HelpText = HelpText;
            this.ArgumentOrder = ArgumentOrder;
            this.aliases = null;
        }

        public ArgumentDefinitionAttribute(string[] aliases, string HelpText = "default help text")
        {
            this.HelpText = HelpText;
            this.ArgumentOrder = -1;
            this.aliases = aliases;
        }
    }
}