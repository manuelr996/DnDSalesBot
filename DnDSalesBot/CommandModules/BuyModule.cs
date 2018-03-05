using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;


namespace DnDSalesBot.CommandModules
{
    class BuyModule : ModuleBase
    {        
        [Command("buy"), Summary("Buy an item listed in the database")]
        public async Task BuyItem([Remainder, Summary("The Item desired to buy")]string itemName)
        {
			String reply = String.Empty, shortDate = DateTime.Now.ToShortDateString();
            Item item = Item.GetFromDatabase(itemName);
			Player buyer = Player.GetFromDatabase(Context.User.DiscriminatorValue);

			//Get the DM channel from the App.Config
			ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out ulong dmChannel);


			if (item != null)
			{
				//Format the Reply
				reply = String.Format("[{0}]: {1} compro **{2}** por **{3}** :crown:", shortDate, Context.User.Mention, item.ItemName, item.ItemPrice);

				if (dmChannel != 0)
					//if the DM channel was parsed successfully then reply to the DM channel
					Utilities.SendMessageAsync(Context, dmChannel, reply);
				else
					//if not then throw an exception
					throw new Exception("bad configuration format please check App.Config");

				Utilities.SendMessageAsync(Context, buyer.PlayerJournalId, reply);
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


		[Command("buy"), Summary("Buy an item listed in the database")]
		public async Task BuyItem() =>
			await ReplyAsync("Wrong Usage: please specify the item you want to buy");

    }
}
