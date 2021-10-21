using System;
using System.Linq;
using System.Text;
using System.Threading;
using SoiBot.Triggers;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class CuilCommand : ICommand
    {
        public CommandTrigger Trigger { get; set; }

        public CuilCommand(CommandTrigger trigger) {
            Trigger = trigger;
        }

        public bool Matches(ChatMessage message, BotVariables variables)
        {
            if (Trigger.Matches(message, variables)) {
                var level = Trigger.GetParameters(message, variables);
                return level.Split(' ').Any(t => Decimal.TryParse(t, out var _));
            }
            return false;
        }

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file)
        {
            var cuilLevel = Decimal.Parse(message.Message.Split(' ').First(t => Decimal.TryParse(t, out var _)));

            WriteResponse(client, "Cuil level registered: ");

            if (cuilLevel >= 5)
            {
                WriteResponse(client, "You ask for a hamburger, I give you a hamburger. You raise it to your lips and take a bite.");
                Thread.Sleep(1000);
                WriteResponse(client, "Your eye twitches involuntarily. Across the street a father of three falls own the stairs.");
                Thread.Sleep(1000);
                WriteResponse(client, "You swallow and look down at the hamburger in your hands. I give you a hamburger.");
                Thread.Sleep(1000);
                WriteResponse(client, "You swallow and look down at the hamburger in your hands. You cannot swallow.");
                Thread.Sleep(1000);
                WriteResponse(client, "There are children at the top of the stairs. A pickle shift uneasily under the bun.");
                Thread.Sleep(1000);
                WriteResponse(client, "I give you a hamburger. You look at my face, and I am pleading with you.");
            }
            else if (cuilLevel >= 4)
            {
                WriteResponse(client, "Why are we speaking German? A mime cries softly as he cradles a young cow.");
            }
            else if (cuilLevel >= 3)
            {
                WriteResponse(client, "You awake as a hamburger. You start screaming only to have special sauce fly from your lips. The world is in sepia.");
            }
            else if (cuilLevel >= 2)
            {
                WriteResponse(client, "You asked me for a hamburger, but I don't really exist. Where I was standing, a picture of a hamburger rests on the ground.");
            }
            else if (cuilLevel >= 1)
            {
                WriteResponse(client, "You asked me for a hamburger, I give you a raccoon.");
            }
            else if (cuilLevel >= 0)
            {
                WriteResponse(client, "You asked me for a hamburger, I give you a hamburger.");
            }
            else
            {
                WriteResponse(client, "This can't be good");
            }
            
            variables.CuilLevel = cuilLevel;
        }

        void WriteResponse(TwitchClient client, string response)
        {
            Console.WriteLine($"= SoiBoiBot: {response}");
            client.SendMessage(Bot.Channel, response);
        }
    }
}