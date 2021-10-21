using TwitchLib.Client.Models;

namespace SoiBot.Triggers
{
    public class FuzzyTrigger : ITrigger
    {
        string triggerWord;

        public FuzzyTrigger(string wordThing)
        {
            triggerWord = wordThing;
        }

        public bool Matches(ChatMessage message, BotVariables variables) => message.Message.ToLowerInvariant().Contains(triggerWord.ToLowerInvariant());
    }
}
