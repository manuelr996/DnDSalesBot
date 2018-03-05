using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class SellModule : ModuleBase
	{
        [Command("sell"), Summary("Vende un item listado en la base de datos")]
        public async Task SellItem(string itemName)
        {
            Item itemSold = Item.GetFromDatabase(itemName);
            Player seller = Player.GetFromDatabase(Context.User.DiscriminatorValue);
            String reply = String.Empty, shortDate = DateTime.Now.ToShortDateString();

            ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel);

			if (seller != null)
			{
				if (itemSold != null)
				{
					reply = String.Format("[{0}]: {1} Vendio **{2}** por **{3}** Coronas", shortDate, Context.User.Mention, itemSold.ItemName, itemSold.ItemPrice);

					if (dmChannel != 0)
						//if the DM channel was parsed successfully then reply to the DM channel
						Utilities.SendMessageAsync(Context, dmChannel, reply);
					else
						//if not then throw an exception
						throw new Exception("bad configuration format please check App.Config");

					Utilities.SendMessageAsync(Context, seller.JournalId, reply);
					//TODO: Log to database
					await ReplyAsync(reply);
				}
				else
				{
					reply = String.Format("[{0}]: El item solicitado no se encontro en la Base de Datos", shortDate);
					string replyDm = String.Format("[{0}]: No se encontro el item: **{1}**\nPara añadir utilice el comando addItem", shortDate, itemName);

					if (dmChannel != 0)
						Utilities.SendMessageAsync(Context, dmChannel, replyDm);

					await ReplyAsync(reply);
					//TODO: Log to database
				}
			}
			else
			{
				reply = String.Format("[{0}]: No se encontro al jugador solicitado", shortDate);
				String replyDm = String.Format("[{0}]: No se encontro al jugador: **{1}**\nPara añadir utilice el comando addPlayer",shortDate, Context.User.Mention);

				if (dmChannel != 0)
					Utilities.SendMessageAsync(Context, dmChannel, replyDm);

				await ReplyAsync(reply);
			}

			//await ReplyAsync("WIP");
		}

    }
}
