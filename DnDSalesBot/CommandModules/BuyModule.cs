using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class BuyModule : ModuleBase
    {        
        [Command("buy"), Summary("Buy an item listed in the database")]
        public async Task BuyItem([Remainder, Summary("The Item desired to buy")]string itemName)
        {
			String reply = String.Empty;
			Item item = Item.GetFromDatabase(itemName);
			string shortDate = DateTime.Now.ToShortDateString();
			ulong dmChannel = 0;

			ulong.TryParse(ConfigurationManager.AppSettings["dmChannel"], out dmChannel);
			//await ReplyAsync("WIP");

			if (item != null)
			{
				reply = String.Format("[{0}]: {1} compro **{2}** por **{3}** Coronas", shortDate, Context.User.Mention, item.itemName, item.itemPrice);

				if (dmChannel != 0)
					SendMessageAsync(dmChannel, reply);
				else
					throw new Exception("bad configuration format please check App.Config");

				//TODO: Notify player Journal
				//TODO: Log to database
				await ReplyAsync(reply);
			}
			else
			{
				reply = String.Format("[{0}]: El item solicitado no se encontro en la Base de Datos", shortDate);
				string replyDm = String.Format("[{0}]: No se encontro el item: **{1}**\nPara añadir utilice el comando addItem", shortDate, itemName);

				if (dmChannel != 0)
					SendMessageAsync(dmChannel, replyDm);

				await ReplyAsync(reply);
				//TODO: Log to database
			}
		}


		[Command("buy"), Summary("Buy an item listed in the database")]
		public async Task BuyItem() =>
			await ReplyAsync("Wrong Usage: please specify the item you want to buy");


		private async void SendMessageAsync(ulong chatId, string message)
		{
			IMessageChannel channel = await Context.Guild.GetTextChannelAsync(chatId);

			await channel.SendMessageAsync(message);
		}
    }
}
