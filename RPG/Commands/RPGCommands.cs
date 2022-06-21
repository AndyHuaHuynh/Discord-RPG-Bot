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
using RPG.Handler.Dialogue.Steps;
using RPG.Handler.Dialogue;
using System.Threading.Channels;
using RPG.DAL;
using RPG.DAL.Models.Items;
using Microsoft.EntityFrameworkCore;
using RPGBot.Core.Services.Items;

namespace RPG.Commands
{
    public class RPGCommands : BaseCommandModule
    {
        private readonly IItemService _itemService;

        public RPGCommands(IItemService itemService)
        {
            _itemService = itemService;
        }

        

        [Command("iteminfo")]

        public async Task ItemInfo(CommandContext ctx)
        {
            var itemNameStep = new TextStep("What item are you looking for?", null); // step to create an input
           
            string itemName = string.Empty;

            itemNameStep.OnValidResult += (result) => itemName = result;

            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, itemNameStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue();

            if (!succeeded)
            {
                return;
            }

            var item = await _itemService.GetItemByName(itemName);

            if (item == null)
            {
                await ctx.Channel.SendMessageAsync($"There is no item called {itemName}");
                return;
            }

            await ctx.Channel.SendMessageAsync($"Name: {item.Name}, Description: {item.Description}");
 
        }



    }

   
 }
