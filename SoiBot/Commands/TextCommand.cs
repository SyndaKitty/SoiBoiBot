using System;
using System.Collections.Generic;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class TextCommand : ICommand
    {
        public ITrigger Trigger { get; set; }
        public List<string> Responses { get; set; } = new List<string>();

        public TextCommand(ITrigger trigger, List<string> responses)
        {
            Trigger = trigger;
            Responses = responses;
        }

        public TextCommand(ITrigger trigger, params string[] responses)
        {
            Trigger = trigger;
            Responses.AddRange(responses);
        }

        public bool Matches(ChatMessage message, BotVariables variables) => Trigger.Matches(message, variables);

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file)
        {
            int responseIndex = new Random().Next(0, Responses.Count);
            Console.WriteLine($"= SoiBoiBot: {Responses[responseIndex]}");
            client.SendMessage(Bot.Channel, Responses[responseIndex]);
        }
    }
}
