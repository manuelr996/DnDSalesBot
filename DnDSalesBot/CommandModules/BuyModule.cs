using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;


namespace DnDSalesBot.CommandModules
{
	class BuyModule : ModuleBase
	{
		#region String Macros
		private const string ITEM_BOUGHT = "[{0}]: {1} compro **{2}** por **{3}** 👑\nDinero Actual de {4}: {5}👑";
		private const string NOT_ENOUGH_GOLD = "{0} no posee suficientes 👑 para realizar la compra";
		private const string ITEM_NOT_FOUND = "No se encontro el item";
		private const string ITEM_NOT_FOUND_DM = "No se encontro el item: **{0}**\nPara añadir utilice el comando addItem";
		#endregion

		#region Commands
		[Command("buy"), Summary("Compra un item listado en la base de datos")]
		public async Task BuyItem([Remainder]string itemName)
		{
			String reply = String.Empty, shortDate = DateTime.Now.ToShortDateString();
			Item item = Item.GetFromDatabase(itemName);
			Player buyer = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			//Get the DM channel from the App.Config and if you can't then throw an exception signalling to check app.config
			if(!ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel))
				throw new Exception(Utilities.BAD_CONFIG);

			if (buyer != null)
			{
				if (item != null)
				{
					if (buyer.Character.CurrentGold >= item.ItemPrice)
					{
						if (buyer.AddGold(-item.ItemPrice))
						{
							//Format the Reply
							reply = String.Format(ITEM_BOUGHT
													, shortDate
													, Context.User.Mention
													, item.ItemName
													, item.ItemPrice
													, Context.User.Mention
													, buyer.Character.CurrentGold);

							Utilities.SendMessageAsync(Context, dmChannel, reply);
							Utilities.SendMessageAsync(Context, buyer.JournalId, reply);

							await ReplyAsync(reply);
						}
						else
							await ReplyAsync(Utilities.UPDATE_FAILED);
					}
					else
						await ReplyAsync(String.Format(NOT_ENOUGH_GOLD, Context.User.Mention));
				}
				else
				{
					reply = String.Format(ITEM_NOT_FOUND);
					string replyDm = String.Format(ITEM_NOT_FOUND_DM, itemName);

					if (dmChannel != 0)
						Utilities.SendMessageAsync(Context, dmChannel, replyDm);

					await ReplyAsync(reply);
				}
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);
		}
		#endregion
	}
}
