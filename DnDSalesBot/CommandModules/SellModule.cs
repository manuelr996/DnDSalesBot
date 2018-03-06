using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class SellModule : ModuleBase
	{
		#region String Macros
		private const string ITEM_SOLD = "[{0}]: {1} Vendio **{2}** por **{3}** 👑\nDinero Actual de {4}: {5}👑";
		private const string ITEM_NOT_FOUND = "No se encontro el item";
		private const string ITEM_NOT_FOUND_DM = "No se encontro el item: **{0}**\nPara añadir utilice el comando addItem";
		#endregion

		#region Commands
		[Command("sell"), Summary("Vende un item listado en la base de datos")]
		public async Task SellItem([Remainder]string itemName)
        {
            Item itemSold = Item.GetFromDatabase(itemName);
            Player seller = Player.GetFromDatabase(Context.User.DiscriminatorValue);
            String reply = String.Empty, shortDate = DateTime.Now.ToShortDateString();

            if(!ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel))
				throw new Exception(Utilities.BAD_CONFIG);

			if (seller != null)
			{
				if (itemSold != null)
				{
					if(seller.AddGold(itemSold.ItemPrice))
					{ 
						reply = String.Format(ITEM_SOLD
												,shortDate
												,Context.User.Mention
												,itemSold.ItemName
												,itemSold.ItemPrice
												,Context.User.Mention
												,seller.Character.CurrentGold);

						Utilities.SendMessageAsync(Context, dmChannel, reply);
						Utilities.SendMessageAsync(Context, seller.JournalId, reply);
						//TODO: Log to database
						await ReplyAsync(reply);
					}
				}
				else
				{
					string replyDm = String.Format(ITEM_NOT_FOUND_DM, itemName);

					if (dmChannel != 0)
						Utilities.SendMessageAsync(Context, dmChannel, replyDm);

					await ReplyAsync(ITEM_NOT_FOUND);
				}
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);
		}
		#endregion
	}
}
