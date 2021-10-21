﻿using System.Media;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class SoundCommand : ICommand
    {
        public ITrigger Trigger { get; set; }
        public string AssetPath { get; set; }

        public SoundCommand(ITrigger trigger, string assetPath)
        {
            Trigger = trigger;
            AssetPath = assetPath;
        }

        public bool Matches(ChatMessage message, BotVariables variables) => Trigger.Matches(message, variables);

        public void Execute(TwitchClient client, ChatMessage message, BotVariables variables, BotFile file)
        {
            SoundPlayer player = new SoundPlayer(AssetPath);
            player.Play();
        }
    }
}
