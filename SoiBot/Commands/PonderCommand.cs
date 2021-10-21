using Newtonsoft.Json.Linq;
using SoiBot.Triggers;
using System;
using System.IO;
using System.Net;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands {
    public class PonderCommand : ICommand {
        public CommandTrigger Trigger { get; set; }
        
        public PonderCommand(CommandTrigger trigger) {
            Trigger = trigger;
        }

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file) {
            string url = @"https://mosaic-api-morality.apps.allenai.org/api/ponder?action1=";
            string scenario = Trigger.GetParameters(message, variables).Trim();
            url += scenario;

            Console.WriteLine(url);

            if (scenario == "") {
                Console.WriteLine("I'd rather not, thanks");
                client.SendMessage(message.Channel, "I'd rather not, thanks");
                return;
            }

            if (scenario.ToLower().Contains("soiboi")) {
                Console.WriteLine("He's cute");
                client.SendMessage(message.Channel, "He's cute");
                return;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                var answer = JObject.Parse(reader.ReadToEnd())["answer"]["text"];
                Console.WriteLine(answer.ToString());
                client.SendMessage(message.Channel, answer.ToString());
            }

        }

        public bool Matches(ChatMessage message, BotVariables variables) {
            return Trigger.Matches(message, variables);
        }
    }
}