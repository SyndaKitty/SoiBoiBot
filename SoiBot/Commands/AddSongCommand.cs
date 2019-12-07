using System.Net;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class AddSongCommand : ICommand
    {
        public ITrigger Trigger { get; set; }

        public AddSongCommand(ITrigger trigger)
        {
            Trigger = trigger;
        }

        public bool Matches(ChatMessage message) => Trigger.Matches(message);

        public void Execute(TwitchClient client, ChatMessage message)
        {
            

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile("", "");
            }


        }
    }
}
