using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace SoiBot.Triggers
{
    public class OnlyTrigger : ITrigger
    {
        public List<string> Words { get; set; } = new List<string>();

        public OnlyTrigger(params string[] words)
        {
            Words.AddRange(words.Select(x => x.ToLowerInvariant()));
        }

        public bool Matches(ChatMessage message, BotVariables variables)
        {
            return Words.Any(w => message.Message.ToLowerInvariant() == w);
        }
    }
}