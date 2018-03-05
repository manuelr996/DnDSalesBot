using System;
using System.Configuration;
using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;
using Discord;

namespace DnDSalesBot.CommandModules
{
    class AddItemModule : ModuleBase
    {
		[Command("additem"),Summary("Adds an Item to the database")]
		public async Task AddItem(string item, double price)
		{
            Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

            if (requester.IsDm)
            {
                if (Item.AddToDatabase(item, price))
                    await ReplyAsync("El objecto fue añadido exitosamente a la base de datos");
                else
                    await ReplyAsync("No se pudo añadir el objeto a la base de datos");
            }
            else
                await ReplyAsync("No se poseen permisos para realizar esta operacion");
        }
    }
}
