using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Models
{
    public abstract class ExposedCommand : Command
    {
        public Tokenizer InputHandle { get; private set; }
        public override void CommandHandle(Console console, Tokenizer tokenizer)
        {
            InputHandle = tokenizer;
            CommandAction();
        }
    }
}
