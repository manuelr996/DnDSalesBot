using System;
using System.Configuration;
using Discord;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class Utilities
    {
		#region String Macros
		public const string PLAYER_NOT_FOUND = "No se encontro al/los jugador/es solicitado/s";
		public const string PLAYER_NOT_FOUND_DM = "No se encontro al jugador: **{0}**\nPara añadir utilice el comando addPlayer";
		public const string BAD_CONFIG = "Hay errores en App.Config";
		public const string INSUFFICIENT_RIGHTS = "No se poseen permisos para realizar la operacion";
		public const string UPDATE_FAILED = "No se pudo realizar la actualizacion en la base de datos";
		#endregion

		public static async void SendMessageAsync(ICommandContext Context, ulong chatId, string message)
        {
            IMessageChannel channel = await Context.Guild.GetTextChannelAsync(chatId);

            await channel.SendMessageAsync(message);
        }

		public static void ReportPlayerNotFound(ICommandContext Context, string mention)
		{
			if (!ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel))
				throw new Exception(BAD_CONFIG);

			String replyDm = String.Format(PLAYER_NOT_FOUND_DM, mention);

			if (dmChannel != 0)
				SendMessageAsync(Context, dmChannel, replyDm);

			SendMessageAsync(Context, Context.Channel.Id, PLAYER_NOT_FOUND);
		}
	}
}
