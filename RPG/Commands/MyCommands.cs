using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using RPG.DAL;
using RPG.DAL.Models.Items;
using RPG.Handler.Dialogue;
using RPG.Handler.Dialogue.Steps;

namespace RPG.Commands
{
    public class MyCommands : BaseCommandModule
    {
        private readonly RPGContext _context;
        public MyCommands(RPGContext context)
        {
            _context = context;
        }

        [Command("additem")]
        public async Task AddItem(CommandContext ctx, string name)
        {
            await _context.Item.AddAsync(new Item { Name = name, Description = "Test Description" });
            await _context.SaveChangesAsync();
        }

        [Command("item")]
        public async Task Item(CommandContext ctx, string name)
        {
            var item = await _context.Item.ToListAsync();
        }

        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("hello")]
        private async Task Hello(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($@"Hello! {ctx.Message.Author.Mention}").ConfigureAwait(false);
        }

        [Command("Age")]
        [Description("Tells the age of author")]
        public async Task Age(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($@"This account was created on {ctx.Message.Author.CreationTimestamp}");
        }

        [Command("time")]
        [Description("Shows the current time")]
        public async Task Time(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($@"The current date and time is {DateTime.Now}");
        }

        [Command ("poll")]
        [Description("Creates a poll. Example: $poll duration title, option, option")]
        public async Task Poll(CommandContext ctx, TimeSpan duration, [RemainingText] string args = "") // takes 
        {
            var interactivity = ctx.Client.GetInteractivity();
            string[] parsedArgs = args.Split(',');
            string message = string.Empty;
            

            int i;
            string loop = "";

            for (i = 1; i < parsedArgs.Length; i++)
            {
                if (i == parsedArgs.Length - 1)
                {
                    loop += parsedArgs[i];
                }
                else
                {
                    loop += parsedArgs[i] + '\n';
                }

            }

            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = parsedArgs[0],
                Description = loop,
                Color = DiscordColor.Blue,
                

            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed);    


        }

        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            var inputStep = new TextStep("Enter something interesting.", null); // step to create an input
            var funnyStep = new TextStep("Haha, funny", null);

            string input = string.Empty;
            int value = 0;

            inputStep.OnValidResult += (result) =>
            {
                input = result;

                if (result == "something interesting")
                {
                    inputStep.SetNextStep(funnyStep);
                }
            }; //when step completed, save step into input


            var userChannel = await ctx.Member.CreateDmChannelAsync();

            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, inputStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue();

            if (!succeeded)
            {
                return;
            }

            await ctx.Channel.SendMessageAsync(input);

        }

        [Command("EmojiDialogue")]
        public async Task EmojiDialogue(CommandContext ctx)
        {

            var yesStep = new TextStep("You chose yes", null);
            var noStep = new TextStep("You chose no", null);
            var emojiStep = new ReactionStep("Yes or No", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client, ":thumbsup:"), new ReactionStepData {Content = "This means yes", NextStep = yesStep } },
                {DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"), new ReactionStepData {Content = "This means no", NextStep = noStep }
            } });

            var userChannel = await ctx.Member.CreateDmChannelAsync();

            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, emojiStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue();

            if (!succeeded)
            {
                return;
            }


       
    }



    }

   
 }
