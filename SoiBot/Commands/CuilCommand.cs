using System;
using System.Linq;
using System.Text;
using SoiBot.Triggers;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class CuilCommand : ICommand
    {
        ITrigger commandTrigger = new CommandTrigger("cuil", "set cuil", "cuil set");
        
        public bool Matches(ChatMessage message, BotVariables variables)
        {
            return commandTrigger.Matches(message, variables)
                   && message.Message.Split(' ').Any(t => Decimal.TryParse(t, out var _));
        }

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file)
        {
            var cuilLevel = Decimal.Parse(message.Message.Split(' ').First(t => Decimal.TryParse(t, out var _)));

            StringBuilder response = new StringBuilder("Cuil level registered: ");

            if (cuilLevel >= 5)
            {
                response.Append("You ask for a hamburger, I give you a hamburger. You raise it to your lips and take a bite. Your eye twitches involuntarily. Across the street a father of three falls own the stairs. You swallow nad look down at the hamburger in your hands. I give you a hamburger. You swallow and look down at the hamburger in your hands. You cannot swallow. There are children at the top of the stairs. A pickle shift uneasily under the bun. I give you a hamburger. You look at my face, and I am pleading with you.");
            }
            else if (cuilLevel >= 4)
            {
                response.Append("Why are we speaking German? A mime cries softly as he cradles a young cow.");
            }
            else if (cuilLevel >= 3)
            {
                response.Append("You awake as a hamburger. You start screaming only to have special sauce fly from your lips. The world is in sepia.");
            }
            else if (cuilLevel >= 2)
            {
                response.Append("You asked me for a hamburger, but I don't really exist. Where I was standing, a picture of a hamburger rests on the ground.");
            }
            else if (cuilLevel >= 1)
            {
                response.Append("You asked me for a hamburger, I give you a raccoon.");
            }
            else if (cuilLevel >= 0)
            {
                response.Append("You asked me for a hamburger, I give you a hamburger.");
            }
            else
            {
                response = new StringBuilder("This can't be good");
            }
            
            variables.CuilLevel = cuilLevel;
            WriteResponse(client, response.ToString());
        }

        void WriteResponse(TwitchClient client, string response)
        {
            Console.WriteLine($"= SoiBoiBot: {response}");
            client.SendMessage(Bot.Channel, response);
        }
    }
}