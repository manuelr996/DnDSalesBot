using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class AddPlayerModule : ModuleBase
    {
		[Command("addplayer"), Summary("Add a player to the database")]
		public async Task AddPlayer(IUser mentionedPlayer)
		{
		}
    }
}
