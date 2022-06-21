using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using RPG.Commands;

namespace RPG.bots
{
    public class Bot
    {

        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public Bot(IServiceProvider services)
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configjson = JsonConvert.DeserializeObject<configjson>(json);


            var config = new DiscordConfiguration()
            {
                Token = configjson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            };
            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration());

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configjson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
                Services = services
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<MyCommands>();
            Commands.RegisterCommands<CoinFlip>();
            Commands.RegisterCommands<RPGCommands>();


            Client.ConnectAsync(); //Connect settings to the discord client gateway
        }

       
        private Task Client_Ready(DiscordClient sender, ReadyEventArgs e) //fires when the discord client is ready 
        {
            return Task.CompletedTask;
        }
    }
}
