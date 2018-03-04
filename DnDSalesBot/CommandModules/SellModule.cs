using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class SellModule : ModuleBase
    {
        private IServiceProvider services;

        [Command("Sell"),Summary("Sell an Item")]
        public async Task SellItem()
        {
			await ReplyAsync("WIP");
		}
    }
}
