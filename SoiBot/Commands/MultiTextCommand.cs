using System;
using System.Collections.Generic;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class MultiTextCommand : ICommand
    {
        public ITrigger Trigger { get; set; }
        public List<string> Responses { get; set; } = new List<string>();

        public MultiTextCommand(ITrigger trigger, List<string> responses)
        {
            Trigger = trigger;
            Responses = responses;
        }

        public MultiTextCommand(ITrigger trigger, params string[] responses)
        {
            Trigger = trigger;
            Responses.AddRange(responses);
        }

        public bool Matches(ChatMessage message, BotVariables variables) => Trigger.Matches(message, variables);

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file)
        {
            for (int responseIndex = 0; responseIndex < Responses.Count; responseIndex++)
            {
                Console.WriteLine($"= SoiBoiBot: {Responses[responseIndex]}");
                client.SendMessage(message.Channel, Responses[responseIndex]);    
            }
        }
    }
}
