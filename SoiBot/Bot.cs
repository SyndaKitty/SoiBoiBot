using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Timers;
using SoiBot.Commands;
using SoiBot.Triggers;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;
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
        
        List<ICommand> recurringCommands = new List<ICommand>();
        const double RecurringInterval = 6 * 60 * 1000;
        int recurringIndex;
        
        TwitchClient client;

        bool startupIgnoreNewFollowers = true;
        
        // TODO bad..
        public static string Channel = "SpencasaurusRex";
        
        public Bot()
        {
            CreateCommands();

            ConnectionCredentials credentials = new ConnectionCredentials("SoiBoiBot", GetAccessToken());
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 100,
                ThrottlingPeriod = TimeSpan.FromSeconds(10)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "SpencasaurusRex");

            //client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            //client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            //client.OnConnected += Client_OnConnected;
            
            var api = new TwitchAPI();
            api.Settings.ClientId = GetClientID();
            api.Settings.Secret = GetSecret();
             
            FollowerService followerService = new FollowerService(api, 10);
            followerService.OnNewFollowersDetected += FollowerService_OnNewFollowersDetected;
            followerService.SetChannelsByName(new List<string> {"SpencasaurusRex"});
            followerService.Start();
            
            client.Connect();

            Timer timer = new Timer(RecurringInterval);
            timer.AutoReset = true;
            timer.Elapsed += SendRecurringCommand;
            timer.Start();
        }

        void FollowerService_OnNewFollowersDetected(Object sender, OnNewFollowersDetectedArgs args)
        {
            // When we first start the service, ALL followers are new followers, so ignore those
            if (startupIgnoreNewFollowers)
            {
                startupIgnoreNewFollowers = false;
                return;
            }

            for (int i = 0; i < args.NewFollowers.Count; i++)
            {
                string message = $"Thank you so much for following @{args.NewFollowers[i].FromUserName}!";
                Console.WriteLine($"= SoiBoiBot: {message}");
                client.SendMessage(args.Channel, message);
            }
        }

        void SendRecurringCommand(object sender, ElapsedEventArgs args)
        {
            if (recurringCommands.Count == 0) return;
            if (recurringIndex >= recurringCommands.Count)
            {
                recurringIndex = 0;
            }

            recurringCommands[recurringIndex++].Execute(client, null);
        }

        void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs args)
        {
            string message = "Have no fear, SoiBoiBot is here :D";
            Console.WriteLine($"= SoiBoiBot: {message}");
            client.SendMessage(args.Channel, message);
        }

        void Client_OnMessageReceived(object sender, OnMessageReceivedArgs args)
        {
            Console.WriteLine($"{args.ChatMessage.DisplayName}: {args.ChatMessage.Message}");
            
            foreach (var command in commands)
            {
                if (command.Matches(args.ChatMessage))
                {
                    try
                    {
                        command.Execute(client, args.ChatMessage);
                    }
                    catch (Exception ex)
                    {
                        string message = "Good job, you broke something @" + args.ChatMessage.Username;
                        Console.WriteLine($"= SoiBoiBot: {message}");
                        client.SendMessage(args.ChatMessage.Channel, message);
                        Console.WriteLine(ex.Message);
                    }
                    return;
                }
            }
        }

        void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs args)
        {
            string message = $"Thank you so much for subscribing @{args.Subscriber.DisplayName}!! ";
            Console.WriteLine($"= SoiBoiBot: {message}");
            client.SendMessage(args.Channel, message);
        }

        void CreateCommands()
        {
            commands.Add(new TextCommand(new CommandTrigger("twitter"), "https://twitter.com/SpencasaurusRex"));
            recurringCommands.Add(new TextCommand(null, "Check out my Twitter! I post there at least once a decade :) https://twitter.com/SpencasaurusRex"));
            
            commands.Add(new TextCommand(new CommandTrigger("discord"), ""));
            recurringCommands.Add(new TextCommand(null, "Be a cutie and join our Discord! https://discord.gg/PH8JwGk"));
            
            commands.Add(new TextCommand(new CommandTrigger("github"), "https://github.com/SpencasaurusRex/"));
            recurringCommands.Add(new TextCommand(null, "You're always welcome to get grossed out by bad code ;) https://github.com/SpencasaurusRex/"));

            var soi = new TextCommand(new CommandTrigger("soi", "soiboi", "soiboibot", "soy"), ":o", ":D", "<3");
            commands.Add(soi);

            var weow = new TextCommand(new FuzzyTrigger("weow"),
                "Meow! 😽",
                "Meow! 🐱",
                "Meow! 🐈",
                "Meow! 😸",
                "Meow! 😹",
                "Meow! 😺",
                "Meow! 😻",
                "Meow! 😼",
                "Meow! 😾",
                "Meow! 🙀");
            commands.Add(weow);
            
            var meow = new TextCommand(new FuzzyTrigger("meow"),
                "Weow! 😽",
                "Weow! 🐱",
                "Weow! 🐈",
                "Weow! 😸",
                "Weow! 😹",
                "Weow! 😺",
                "Weow! 😻",
                "Weow! 😼",
                "Weow! 😾",
                "Weow! 🙀"
            );
            commands.Add(meow);
            
            var sayKiss = new TextCommand(null, "😽");
            var playKiss = new SoundCommand(null, @"C:\Stream\kiss.wav");
            commands.Add(new CompositeCommand(new PaddedTrigger(":*", "💋", "😗", "😘", "😙", "😚", "😽"), false, sayKiss, playKiss));

            var fax = new TextCommand(new CommandTrigger("fax", "facts"), "He spittin str8 𝐅 Å 𝕏");
            commands.Add(fax);
            recurringCommands.Add(new CompositeCommand(null, true, weow, meow, sayKiss, fax, soi));
            
            // TODO make this actually store/retrieve quote info
            commands.Add(new TextCommand(new CommandTrigger("quote"), "Quoteth", "Quote"));
            
            var sayMoo = new TextCommand(null, "Moo!", "🐄🐄🐄");
            var playMoo = new SoundCommand(null, @"C:\Stream\moo.wav");
            commands.Add(new CompositeCommand(new CommandTrigger("moo"), false, sayMoo, playMoo));

            // My lovely friends :)
            commands.Add(new TextCommand(new CommandTrigger("false", "falsebracket"), "falsebracket.com"));
            commands.Add(new TextCommand(new CommandTrigger("dread", "dreadusa"), "You know not what power you invoke when speaking that name..."));
            // commands.
            
            commands.Add(new Magic8BallCommand(new CommandTrigger("8ball")));
            
            // TODO: Broken :(
            // commands.Add(new AddSongCommand(new CommandTrigger("addsong", "downloadsong")));

            commands.Add(new TextCommand(new PaddedTrigger("haha"), "hehe"));
            commands.Add(new TextCommand(new PaddedTrigger("hehe"), "haha"));
            
            commands.Add(new TextCommand(new PaddedTrigger("o/", @"\o", "HeyGuys"), "HeyGuys"));
            
            var help = new MultiTextCommand(new CommandTrigger("help", "commands"),
                "!soi : That's me!",
                "!8ball : Receive my divine wisdom",
                "!moo : 🐄"
                );
            commands.Add(help);
            
            List<string> thankVariations = new List<string> { "thanks", "ty", "thank you", "thankyou" };
            List<string> soiNameVariations = new List<string> { "soi", "soiboi", "soiboibot", "@soiboibot"};

            var ty = thankVariations.SelectMany(x => soiNameVariations.Select(y => x + " " + y)).ToArray();
            commands.Add(new TextCommand(new PaddedTrigger(ty), "😸", "You're welcome :D"));

            List<string> meanCommands = new List<string> {"gtfo", "leave", "shut up", "shush", "stfu"};
            var meanCommandTriggers = meanCommands.SelectMany(x => soiNameVariations.Select(y => x + " " + y)).ToArray();
            commands.Add(new TextCommand(new PaddedTrigger(meanCommandTriggers), "😿", "NO! 😾", "Bite me 😾", "[╯°□°]╯︵ ┻━┻"));

            List<string> meanWords = new List<string> { "i hate you", "you suck" };
            var meanWordTriggers = meanWords.SelectMany(x => soiNameVariations.Select(y => x + " " + y)).ToArray();
            commands.Add(new TextCommand(new PaddedTrigger(meanWordTriggers), "😿"));

            commands.Add(new TextCommand(new PaddedTrigger("RIP"), "F", "𝓕", "𝑓"));
        }

        string GetAccessToken()
        {
            return File.ReadAllText(ConfigurationManager.AppSettings["AccessTokenFile"]);
        }

        string GetClientID()
        {
            return File.ReadAllText(ConfigurationManager.AppSettings["ClientIDFile"]);
        }

        string GetSecret()
        {
            return File.ReadAllText(ConfigurationManager.AppSettings["SecretFile"]);
        }
    }
}
