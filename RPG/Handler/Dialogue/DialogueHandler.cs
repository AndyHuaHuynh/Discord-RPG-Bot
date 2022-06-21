using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using RPG.Handler.Dialogue.Steps;

namespace RPG.Handler.Dialogue
{
    public class DialogueHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IDialogueStep _currentStep;


        public DialogueHandler(DiscordClient client, DiscordChannel channel, DiscordUser user, IDialogueStep startingStep)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _currentStep = startingStep;
        }

        private readonly List<DiscordMessage> messages = new List<DiscordMessage>(); //list of messages

        public async Task<bool> ProcessDialogue()
        {
            while(_currentStep != null)
            {
                _currentStep.OnMessageAdded += (message) => messages.Add(message); //when a message is sent, add to the list

                bool canceled = await _currentStep.ProcessStep(_client, _channel, _user);

                if (canceled)
                {
                    await DeleteMessages();

                    var cancelEmbed = new DiscordEmbedBuilder
                    {
                        Title = "Dialogue Canceled",
                        Description = _user.Mention,
                        Color = DiscordColor.Green,
                    };

                    await _channel.SendMessageAsync(embed: cancelEmbed);

                    return false;
                }

                _currentStep = _currentStep.NextStep;
            }

            await DeleteMessages();

            return true;
        }

        private async Task DeleteMessages()
        {
            if (_channel.IsPrivate)
            {
                return;

                foreach(var message in messages)
                {
                    await message.DeleteAsync();
                }
            }
        }

    }
}
