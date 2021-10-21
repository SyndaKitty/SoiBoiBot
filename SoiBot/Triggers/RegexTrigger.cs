using System.Collections.Generic;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;

namespace SoiBot.Triggers
{
    public class RegexTrigger : ITrigger
    {
        public List<string> Patterns { get; set; } = new List<string>();

        public RegexTrigger(params string[] patterns)
        {
            Patterns.AddRange(patterns);
        }
        
        public bool Matches(ChatMessage message, BotVariables variables)
        {
            foreach (var pattern in Patterns)
            {
                if (Regex.IsMatch(message.Message, pattern))
                    return true;
            }

            return false;
        }

        public bool Matches(ChatMessage message, out Match data)
        {
            data = null;
            foreach (var pattern in Patterns)
            {
                data = Regex.Match(message.Message, pattern);
                if (data != null)
                    return true;
            }
            
            return false;
        }
    }
}