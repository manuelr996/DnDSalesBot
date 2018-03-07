using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class ConsultModule : ModuleBase
    {
		#region String Macros
		private const string PLAYER_DATA = "Nombre del Jugador: {0}\nNombre del Personaje: {1}\n:crown:: {2}";
		#endregion
		[Command("getplayer"),Summary("Recupera los datos del jugador especificado")]
		public async Task GetPlayer(IGuildUser player)
		{
			Player result = Player.GetFromDatabase(player.DiscriminatorValue);

			if (result != null)
			{
				string dmStatus = result.IsDm ? "DM" : "Jugador";
				await ReplyAsync(String.Format(PLAYER_DATA, player.Mention, result.Character.Name, result.Character.CurrentGold));
			}
			else
				Utilities.ReportPlayerNotFound(Context, player.Mention);
		}

		[Command("updateJournal"), Summary("Actualiza el Journal del jugador que invoca")]
		public async Task UpdateJournal()
		{
			IGuildUser player = ((IGuildUser)Context.User);
			Player requester = Player.GetFromDatabase(player.DiscriminatorValue);

			if(requester != null)
			{
				
			}
			
		}
    }
}
