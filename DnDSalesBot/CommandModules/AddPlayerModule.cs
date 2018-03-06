using System;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class AddPlayerModule : ModuleBase
    {
		#region String Macros
		private const string PLAYER_ADDED = "El jugador fue añadido exitosamente a la base de datos";
		private const string ADDING_FAILED = "No se pudo añadir al jugador a la base de datos";
		#endregion

		#region Commands
		[Command("addplayer"), Summary("Añadir un jugador a la base de datos (solo DMs)")]
		public async Task AddPlayer(IGuildUser mentionedPlayer, ulong journalId)
		{
			Player newPlayer = new Player();
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			newPlayer.Discriminator = mentionedPlayer.DiscriminatorValue;
			newPlayer.Name = mentionedPlayer.Username;
			newPlayer.JournalId = journalId;
			if (requester != null)
			{
				if (requester.IsDm)
				{
					if (Player.AddToDatabase(newPlayer))
						await ReplyAsync(PLAYER_ADDED);
					else
						await ReplyAsync(ADDING_FAILED);
				}
				else
					await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);

		}

		[Command("addplayer"), Summary("Añadir un jugador a la base de datos especificando el nombre del personaje (solo DMs)")]
		public async Task AddPlayer(IGuildUser mentionedPlayer, string characterName, ulong journalId)
		{
			Player newPlayer = new Player();
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			newPlayer.Discriminator = mentionedPlayer.DiscriminatorValue;
			newPlayer.Name = mentionedPlayer.Username;
			newPlayer.JournalId = journalId;
			newPlayer.Character.Name = characterName;

			if (requester.IsDm)
			{
				if (Player.AddToDatabase(newPlayer))
					await ReplyAsync(PLAYER_ADDED);
				else
					await ReplyAsync(ADDING_FAILED);
			}
			else
				await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
		}

		[Command("addplayer"), Summary("Añadirse a uno mismo como jugador (solo administradores)")]
		public async Task AddPlayer(ulong journalId)
		{
			IGuildUser requester = ((IGuildUser)Context.User);
			GuildPermissions permissions = requester.GuildPermissions;
			Player newPlayer = new Player();

			if (permissions.Administrator)
			{
				newPlayer.Name = Context.User.Username;
				newPlayer.Discriminator = Context.User.DiscriminatorValue;
				newPlayer.JournalId = journalId;

				if (Player.AddToDatabase(newPlayer))
					await ReplyAsync(PLAYER_ADDED);
				else
					await ReplyAsync(ADDING_FAILED);
			}
			else
				await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
		}
		#endregion
	}
}
