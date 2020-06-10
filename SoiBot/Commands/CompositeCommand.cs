using System;
using System.Collections.Generic;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class CompositeCommand : ICommand
    {
        ITrigger Trigger { get; set; }
        List<ICommand> Commands { get; set; } = new List<ICommand>();

        public bool Random { get; set; }

        public CompositeCommand(ITrigger trigger, bool random, params ICommand[] commands)
        {
            Trigger = trigger;
            Commands.AddRange(commands);
            Random = random;
        }

        public bool Matches(ChatMessage message) => Trigger.Matches(message);

        public void Execute(TwitchClient client, ChatMessage message)
        {
            if (Random)
            {
                int commandIndex = new Random().Next(0, Commands.Count);
                var command = Commands[commandIndex];
                command.Execute(client, message);
                return;
            }

            foreach (var command in Commands)
            {
                command.Execute(client, message);
            }
        }
    }
}
