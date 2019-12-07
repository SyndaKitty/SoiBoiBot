using System.Collections.Generic;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class CompositeCommand : ICommand
    {
        ITrigger Trigger { get; set; }
        List<ICommand> Commands { get; set; } = new List<ICommand>();

        public CompositeCommand(ITrigger trigger, params ICommand[] commands)
        {
            Trigger = trigger;
            Commands.AddRange(commands);
        }

        public bool Matches(ChatMessage message) => Trigger.Matches(message);

        public void Execute(TwitchClient client, ChatMessage message)
        {
            foreach (var command in Commands)
            {
                command.Execute(client, message);
            }
        }
    }
}
