using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class AddItemModule : ModuleBase
    {
		[Command("additem"),Summary("Adds an Item to the database")]
		public async Task AddItem(string item, decimal price)
		{
			await ReplyAsync("WIP");
		}
    }
}
