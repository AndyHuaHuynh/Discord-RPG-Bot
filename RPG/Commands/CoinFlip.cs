using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Discord;
using Discord.WebSocket;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using Discord.Addons.Interactive;

namespace RPG.Commands
{
    public class CoinFlip : BaseCommandModule
    {
        [Command("Coinflip")]
        public async Task Coinflip(CommandContext ctx)
        {
            var coinEmbed = new DiscordEmbedBuilder
            {
                Title = "Heads or Tails",
                Color = DiscordColor.Blue
            };

            var coinMessage = await ctx.Channel.SendMessageAsync(embed: coinEmbed);

            Random coinFlip = new Random();
            int result = coinFlip.Next(1, 3);
      

            var interactivity = ctx.Client.GetInteractivity();

            var response = await interactivity.WaitForMessageAsync
                (x => x.Channel == ctx.Channel);

            if (response.Result.Content.ToUpper() == "HEADS" || response.Result.Content.ToUpper() == "H")
            {
                if (result == 1)
                {
                    await ctx.Channel.SendMessageAsync("Flip: Heads, you won");
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("Flip: Tails, you lost");
                }
            }
            if (response.Result.Content.ToUpper() == "TAILS" || response.Result.Content.ToUpper() == "T")
            {
                if (result == 1)
                {
                    await ctx.Channel.SendMessageAsync("Flip: Heads, you lost");
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("Flip: Tails, you won");
                }

            }
     


         }

    }
       
   
 }
