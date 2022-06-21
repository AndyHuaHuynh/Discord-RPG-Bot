using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace RPG.Handler.Dialogue.Steps
{
    public class ReactionStep : DialogueStep
    {
        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };
        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please react to this embed",
                Description = $"{user.Mention}, {_content}",
            };
            
            embedBuilder.AddField("To stop the dialogue", "React with the :x: emoji");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder);

                OnMessageAdded(embed);

                foreach (var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji);
                }

                await embed.CreateReactionAsync(cancelEmoji);

                var reactionResult = await interactivity.WaitForReactionAsync(x => _options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji, embed, user); //stops user from reacting with unwanted emoji

                if(reactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                _selectedEmoji = reactionResult.Result.Emoji;

                OnValidResult(_selectedEmoji);

                return false;


            }

        }
    }

    public class ReactionStepData
    {
        public IDialogueStep NextStep { get; set; }
        public string Content { get; set; }
    }
}
