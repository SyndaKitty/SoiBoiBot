using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands {
    public class ChemicalsCommand : ICommand {
        ITrigger Trigger { get; set; }

        public ChemicalsCommand(ITrigger trigger) {
            Trigger = trigger;
        }
        
        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file) {
            file.RecordVariable("Gayness", 1);
            new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(15000);
                file.RecordVariable("Gayness", 0);
            }).Start();

            client.SendMessage(Bot.Channel, "🌈🐸🌈");
        }

        public bool Matches(ChatMessage message, BotVariables variables) {
            return Trigger.Matches(message, variables);
        }
    }
}
