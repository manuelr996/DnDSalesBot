using System;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class AddPlayerModule : ModuleBase
    {
		[Command("addplayer"), Summary("Añadir un jugador a la base de datos (solo DMs)")]
		public async Task AddPlayer(IGuildUser mentionedPlayer, ulong journalId)
		{
			Player newPlayer = new Player();
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			newPlayer.Discriminator = mentionedPlayer.DiscriminatorValue;
			newPlayer.Name = mentionedPlayer.Username;
			newPlayer.JournalId = journalId;
			newPlayer.Character.Name = String.Empty;

			if (requester.IsDm)
			{
				if (Player.AddToDatabase(newPlayer))
					await ReplyAsync("El jugador fue añadido exitosamente a la base de datos");
				else
					await ReplyAsync("No se pudo añadir al jugador a la base de datos");
			}
			else
				await ReplyAsync("No se poseen permisos para realizar la operacion");

		}
    }
}
