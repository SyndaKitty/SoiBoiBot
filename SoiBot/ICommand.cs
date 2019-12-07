using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot
{
    public interface ICommand
    {
        bool Matches(ChatMessage message);
        void Execute(TwitchClient client, ChatMessage message);
    }
}
