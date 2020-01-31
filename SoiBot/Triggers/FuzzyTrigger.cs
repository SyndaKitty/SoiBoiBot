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

        public bool Matches(ChatMessage message) => message.Message.ToLowerInvariant().Contains(triggerWord.ToLowerInvariant());
    }
}
