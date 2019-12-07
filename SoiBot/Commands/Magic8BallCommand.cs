using System;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class Magic8BallCommand : ICommand
    {
        ITrigger Trigger { get; set; }

        public Magic8BallCommand(ITrigger trigger)
        {
            Trigger = trigger;
        }

        public bool Matches(ChatMessage message) => Trigger.Matches(message);

        public void Execute(TwitchClient client, ChatMessage message)
        {
            string[] responses =
            {
                "It is certain.",
                "It is decidedly so.",
                "Yes - definitely,",
                "You may rely on it.",
                "As I see it, yes",
                "Most likely",
                "Outlook good.",
                "Yes.",
                "Signs point to yes",

                "Reply hazy, try again.",
                "Ask again later.",
                "Better not tell you now.",
                "Cannot predict now.",
                "Concentrate and ask again.",

                "Don't count on it.",
                "My reply is no.",
                "My sources say no.",
                "Outlook not so good.",
                "Very doubtful."
            };

            int responseIndex = new Random().Next(0, responses.Length);
            Console.WriteLine($"= SoiBoiBot: {responses[responseIndex]}");
            client.SendMessage(message.Channel, responses[responseIndex]);
        }
    }
}
