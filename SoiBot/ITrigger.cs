using TwitchLib.Client.Models;

namespace SoiBot
{
    public interface ITrigger
    {
        bool Matches(ChatMessage message);
    }
}
