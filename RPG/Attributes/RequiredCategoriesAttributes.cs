using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace RPG.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)] // useable on methods and classes
    public class RequiredCategoriesAttributes : CheckBaseAttribute
    {
        
        public IReadOnlyList<string> CategoryNames { get; }
        public ChannelCheckMode CheckMode { get;  }

        public RequiredCategoriesAttributes(ChannelCheckMode checkMode, params string [] channelName)
        {
            CheckMode = checkMode;
            CategoryNames = new ReadOnlyCollection<string>(channelName);
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if(ctx.Guild == null || ctx.Member == null)
            {
                return Task.FromResult(false);
            }

            bool contains = CategoryNames.Contains(ctx.Channel.Parent.Name, StringComparer.OrdinalIgnoreCase);
            return CheckMode switch
            {
                ChannelCheckMode.Any => Task.FromResult(contains),
                ChannelCheckMode.None => Task.FromResult(!contains),
                _ => Task.FromResult(false),

            };
        }
    }
}
