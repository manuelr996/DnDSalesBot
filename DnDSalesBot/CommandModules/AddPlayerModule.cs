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
		private const string PLAYER_ALREADY_EXISTS = "El jugador ya existe en la base de datos";
		private const string ADDING_FAILED = "No se pudo añadir al jugador a la base de datos";
		private const string NAME_UPDATED = "Se actualizo el nombre del jugador";
		private const string UPDATE_FAILED = "No se pudo actualizar el nombre del jugador";
		#endregion

		#region Commands
		[Command("addplayer"), Summary("Añadir un jugador a la base de datos (solo DMs)")]
		public async Task AddPlayer(IGuildUser mentionedPlayer, ulong journalId)
		{
			Player newPlayer = Player.GetFromDatabase(mentionedPlayer.DiscriminatorValue);
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			if (newPlayer == null)
			{
				newPlayer = new Player();

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
			else
				await ReplyAsync(PLAYER_ALREADY_EXISTS);
				

		}

		[Command("addplayer"), Summary("Añadir un jugador a la base de datos especificando el nombre del personaje (solo DMs)")]
		public async Task AddPlayer(IGuildUser mentionedPlayer, ulong journalId,[Remainder] string characterName)
		{
			Player newPlayer = Player.GetFromDatabase(mentionedPlayer.DiscriminatorValue);
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			if (newPlayer == null)
			{
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
			else
				await ReplyAsync(PLAYER_ALREADY_EXISTS);
		}

		[Command("addplayer"), Summary("Añadirse a uno mismo como jugador (solo administradores)")]
		public async Task AddPlayer(ulong journalId)
		{
			IGuildUser requester = ((IGuildUser)Context.User);
			GuildPermissions permissions = requester.GuildPermissions;
			Player newPlayer = Player.GetFromDatabase(requester.DiscriminatorValue);

			if (newPlayer == null)
			{
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
			else
				await ReplyAsync(PLAYER_ALREADY_EXISTS);
		}

		[Command("addplayer"), Summary("Añadirse a uno mismo como jugador especificando el nombre del personaje (solo administradores)")]
		public async Task AddPlayer(ulong journalId,[Remainder] string characterName)
		{
			IGuildUser requester = ((IGuildUser)Context.User);
			GuildPermissions permissions = requester.GuildPermissions;
			Player newPlayer = Player.GetFromDatabase(requester.DiscriminatorValue);

			if (newPlayer == null)
			{
				if (permissions.Administrator)
				{
					newPlayer.Name = Context.User.Username;
					newPlayer.Discriminator = Context.User.DiscriminatorValue;
					newPlayer.JournalId = journalId;
					newPlayer.Character.Name = characterName;

					if (Player.AddToDatabase(newPlayer))
						await ReplyAsync(PLAYER_ADDED);
					else
						await ReplyAsync(ADDING_FAILED);
				}
				else
					await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
			}
			else
				await ReplyAsync(PLAYER_ALREADY_EXISTS);
		}

		[Command("setcharactername"),Summary("Setea el nombre de personaje de un jugador")]
		public async Task SetCharacterName(IGuildUser mentionedPlayer,[Remainder] string characterName)
		{
			Player playerUpdate = Player.GetFromDatabase(mentionedPlayer.DiscriminatorValue);

			if (playerUpdate != null)
			{
				if (playerUpdate.SetCharacterName(characterName))
					await ReplyAsync(NAME_UPDATED);
				else
					await ReplyAsync(UPDATE_FAILED);				
			}
			else
				Utilities.ReportPlayerNotFound(Context, mentionedPlayer.Mention);
			
		}
		#endregion
	}
}
