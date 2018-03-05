using Discord;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class Utilities
    {
        public static async void SendMessageAsync(ICommandContext Context, ulong chatId, string message)
        {
            IMessageChannel channel = await Context.Guild.GetTextChannelAsync(chatId);

            await channel.SendMessageAsync(message);
        }
    }
}
