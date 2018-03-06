using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class MakeDmModule : ModuleBase
    {
		#region String Macros
		private const string DM_UPDATED = "Se convirtio a {0} en DM";
		private const string ALREADY_DM = "El jugador que se intenta añadir ya es DM";
		#endregion
		#region Commands
		[Command("makeDm"), Summary("le da al jugador permisos de Dm para con el bot (solo DM)")]
		public async Task MakeDm(IGuildUser member)
		{
			Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);
			Player memberPlayer = Player.GetFromDatabase(member.DiscriminatorValue);
			//ulong dmRoleId = 0;

			if (requester != null)
			{
				if (memberPlayer != null)
				{

					if (requester.IsDm) // && ulong.TryParse(ConfigurationManager.AppSettings["dmRoleId"], out dmRoleId))
					{
						if (!memberPlayer.IsDm)
						{
							if (memberPlayer.UpdateDM(true))
								await ReplyAsync(String.Format(DM_UPDATED,member.Mention));
							else
								await ReplyAsync(Utilities.UPDATE_FAILED);
						}
						else
							await ReplyAsync(ALREADY_DM);
					}
					else
						await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
				}
				else
					Utilities.ReportPlayerNotFound(Context, member.Mention);
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);
		}

		[Command("makeDm"), Summary("da permisos de Dm al invocador (solo administrador)")]
		public async Task MakeDm()
		{
			IGuildUser requester = ((IGuildUser) Context.User);
			Player reqPlayer = Player.GetFromDatabase(Context.User.DiscriminatorValue);
			GuildPermissions permissions = requester.GuildPermissions;

			if (reqPlayer != null)
			{
				if (!reqPlayer.IsDm)
				{
					if (permissions.Administrator)
					{
						if (reqPlayer.UpdateDM(true))
							await ReplyAsync(String.Format(DM_UPDATED, requester.Mention));
						else
							await ReplyAsync(Utilities.UPDATE_FAILED);
					}
					else
						await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
				}
				else
					await ReplyAsync(ALREADY_DM);
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);

		}
		#endregion
	}
}
