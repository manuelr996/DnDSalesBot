using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DnDSalesBot.Object_Layer;

namespace DnDSalesBot.CommandModules
{
    class GiveGoldModule : ModuleBase
    {
		private const string GOLD_GIVEN = "[{0}]: se le otorgó **{1}** 👑 a **{2}**\nDinero actual de {3}: {4} 👑";
		[Command("givegold"), Summary("Da una cierta cantidad de coronas a un jugador (Solo DM)")]
		public async Task GiveMoney(IGuildUser moneyRecipient, double goldAmount)
		{
			Player recipientPlayer = Player.GetFromDatabase(moneyRecipient.DiscriminatorValue);
			Player givingPlayer = Player.GetFromDatabase(Context.User.DiscriminatorValue);
			string shortTime = DateTime.Now.ToShortDateString();

			if (!ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel))
				throw new Exception(Utilities.BAD_CONFIG);

			if (givingPlayer != null)
			{
				if (recipientPlayer != null)
				{
					if (givingPlayer.IsDm)
					{
						if (recipientPlayer.AddGold(goldAmount))
						{
							string reply = String.Format(GOLD_GIVEN, shortTime, goldAmount, moneyRecipient.Mention,moneyRecipient.Mention,recipientPlayer.Character.CurrentGold);

							await ReplyAsync(reply);
							Utilities.SendMessageAsync(Context, dmChannel, reply);
							Utilities.SendMessageAsync(Context, recipientPlayer.JournalId, reply);
						}
						else
							await ReplyAsync(Utilities.UPDATE_FAILED);
					}
					else
						await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
				}
				else
					Utilities.ReportPlayerNotFound(Context, moneyRecipient.Mention);
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);
		}
    }
}
