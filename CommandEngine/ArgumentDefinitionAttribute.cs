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
        /// Flags if the command argument is necessary for execution
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Argument tooltip
        /// </summary>
        public string HelpText { get; }

        /// <summary>
        /// Order in the argument list
        /// </summary>
        public int ArgumentOrder { get; }

        /// <summary>
        /// Flags if the argument key should be typed out in full, or if the value should be inferred from the position of itself in the argument list
        /// </summary>
        public bool Verbose => ArgumentOrder != -1;

        public ArgumentDefinitionAttribute(bool Required = false, string HelpText = "default help text")
        {
            this.Required = Required;
            this.HelpText = HelpText;
            this.ArgumentOrder = -1;
        }
    }
}
