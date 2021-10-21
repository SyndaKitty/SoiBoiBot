using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;

namespace SoiBot.Triggers
{
    public class PaddedTrigger : ITrigger
    {
        public List<string> Words { get; set; } = new List<string>();

        public PaddedTrigger(params string[] words)
        {
            Words.AddRange(words.Select(x => x.ToLowerInvariant()));
        }

        public bool Matches(ChatMessage message, BotVariables variables)
        {
            var lowerMessage = message.Message.ToLowerInvariant().Replace("?", "").Replace("!", "");

            return Words.Any(x => (lowerMessage.StartsWith(x) && lowerMessage.EndsWith(x)) || lowerMessage.StartsWith($"{x} ") || lowerMessage.Contains($" {x} ") || lowerMessage.EndsWith($" {x}"));
        }
    }
}
