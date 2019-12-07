using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using SoiBot.Commands;
using SoiBot.Triggers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace SoiBot
{
    public class Bot
    {
        List<ICommand> commands = new List<ICommand>();

        TwitchClient client;

        public Bot()
        {
            CreateCommands();

            ConnectionCredentials credentials = new ConnectionCredentials("SoiBoiBot", GetAccessToken());
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "SpencasaurusRex");

            //client.OnLog += Client_OnLog;
            //client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            //client.OnWhisperReceived += Client_OnWhisperReceived;
            //client.OnNewSubscriber += Client_OnNewSubscriber;
            //client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        void Client_OnMessageReceived(object sender, OnMessageReceivedArgs args)
        {
            foreach (var command in commands)
            {
                if (command.Matches(args.ChatMessage))
                {
                    command.Execute(client, args.ChatMessage);
                    return;
                }
            }

            Console.WriteLine($"{args.ChatMessage.DisplayName}: {args.ChatMessage.Message}");
        }

        void CreateCommands()
        {
            #region CommandTriggers

            // TODO make this actually store/retrieve quote info
            commands.Add(new TextCommand(new CommandTrigger("quote"), "Quoteth", "Quote"));
            
            var sayMoo = new TextCommand(null, "Moo!");
            var playMoo = new SoundCommand(null, @"C:\Stream\moo.wav");
            commands.Add(new CompositeCommand(new CommandTrigger("moo"), sayMoo, playMoo));

            commands.Add(new TextCommand(new CommandTrigger("twitter"), "https://twitter.com/SpencasaurusRex"));

            commands.Add(new TextCommand(new CommandTrigger("soi", "soiboi", "soiboibot", "soy"), ":o"));

            // My lovely friends :)
            commands.Add(new TextCommand(new CommandTrigger("false", "falsebracket"), " // TODO"));
            commands.Add(new TextCommand(new CommandTrigger("dread", "dreadusa"), "You know not what power you invoke when speaking that name..."));

            commands.Add(new Magic8BallCommand(new CommandTrigger("8ball")));
            #endregion

            #region ChatTrigger

            commands.Add(new TextCommand(new ChatTrigger("haha"), "hehe"));
            commands.Add(new TextCommand(new ChatTrigger("hehe"), "haha"));
            
            var sayKiss = new TextCommand(null, "😽");
            var playKiss = new SoundCommand(null, @"C:\Stream\kiss.wav");
            commands.Add(new CompositeCommand(new ChatTrigger(":*", "💋", "😗", "😘", "😙", "😚", "😽"), sayKiss, playKiss));
            commands.Add(new TextCommand(new ChatTrigger("o/", @"\o"), "HeyGuys"));

            #endregion
        }

        string GetAccessToken()
        {
            return File.ReadAllText(ConfigurationManager.AppSettings["AccessTokenFile"]);
        }
    }
}
