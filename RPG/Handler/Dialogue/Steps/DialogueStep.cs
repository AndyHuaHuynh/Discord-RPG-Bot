﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace RPG.Handler.Dialogue.Steps
{
    public abstract class DialogueStep : IDialogueStep

    {
        protected readonly string _content;

        public DialogueStep(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract IDialogueStep NextStep { get; } 

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please try again",
                Color = DiscordColor.Red,
            };

            embedBuilder.AddField("There was a problem with your input", problem);

            var embed = await channel.SendMessageAsync(embed: embedBuilder);

            OnMessageAdded(embed);
        }
    }
}
