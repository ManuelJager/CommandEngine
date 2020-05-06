namespace CommandEngine.Models
{
    /// <summary>
    /// Command instance.
    /// Has no state
    /// </summary>
    public class Command
    {
        private string helpText;

        public string HelpText
        {
            get => helpText == "" ? "no help text provided" : helpText;
            set => helpText = value;
        }

        public Command(string helpText)
        {
            this.HelpText = helpText;
        }
    }
}