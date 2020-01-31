using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace SoiBot.Commands
{
    public class AddSongCommand : ICommand
    {
        public ITrigger Trigger { get; set; }

        public AddSongCommand(ITrigger trigger)
        {
            Trigger = trigger;
        }

        public bool Matches(ChatMessage message) => Trigger.Matches(message);

        public void Execute(TwitchClient client, ChatMessage message)
        {
            // https://beatsaver.com/beatmap/72de
            // https://beatsaver.com/api/download/key/72de
            // https://bsaber.com/songs/72de/
            // 72de

            var pattern =
                @"^(?:(?:https:\/\/)?(?:(?:beatsaver\.com\/api\/download\/key\/)|(?:beatsaver\.com\/beatmap\/)|(?:bsaber\.com\/songs\/)))?([A-Z0-9]{3,4})$";

            var tokens = message.Message.TrimStart().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 2)
            {
                client.SendMessage(message.Channel, "🤔");
                return;
            }

            string link = tokens[1];
            link = link.TrimEnd('/');

            // Make sure its a valid link
            if (!Regex.IsMatch(link, pattern, RegexOptions.IgnoreCase))
            {
                client.SendMessage(message.Channel, "🤔");
                return;
            }

            var match = Regex.Match(link, pattern, RegexOptions.IgnoreCase);
            DownloadFile("https://beatsaver.com/api/download/key/" + match.Groups[1].Value);
        }

        public async void DownloadFile(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var response = await request.GetResponseAsync();
            Console.WriteLine("Calling " + response.ResponseUri.OriginalString);
            
            var filename = response.ResponseUri.OriginalString.Split('/').Last();

            using (WebClient client = new WebClient())
            {
                string customLevels = @"F:\SteamLibrary\steamapps\common\Beat Saber\Beat Saber_Data\CustomLevels\";
                string downloadPath = customLevels + filename;
                string extractPath = customLevels + filename.Replace(".zip", "");
                client.DownloadFile(response.ResponseUri.OriginalString, downloadPath);
                if (!Directory.Exists(extractPath))
                {
                    ZipFile.ExtractToDirectory(downloadPath, extractPath);
                }
                // Delete .zip
                File.Delete(downloadPath);
            }
        }
    }
}
