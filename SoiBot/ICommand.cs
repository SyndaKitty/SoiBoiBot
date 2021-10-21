using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot
{
    public interface ICommand
    {
        bool Matches(ChatMessage message, BotVariables variables);
        void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file);
    }
}
