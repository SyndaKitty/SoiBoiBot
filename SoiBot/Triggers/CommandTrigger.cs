using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace SoiBot.Triggers
{
    public class CommandTrigger : ITrigger
    {
        const string CommandPrefix = "!";

        public List<string> Words { get; set; } = new List<string>();

        public CommandTrigger(List<string> words)
        {
            Words = words.Select(x => x.ToLowerInvariant()).ToList();
        }

        public CommandTrigger(params string[] words)
        {
            Words.AddRange(words.Select(x => x.ToLowerInvariant()));
        }

        public bool Matches(ChatMessage message)
        {
            var text = message.Message.TrimStart().ToLowerInvariant();
            return Words.Any(x => text.StartsWith(CommandPrefix + x + " ") || text.StartsWith(CommandPrefix + x) && text.EndsWith(CommandPrefix + x));
        }
    }
}
