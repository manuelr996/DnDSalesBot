using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class AddItemModule : ModuleBase
    {
		[Command("additem"),Summary("Añade un item a la base de datos (solo DMs)")]
		public async Task AddItem(string item, double price)
		{
            Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);

            if (requester.IsDm)
            {
                if (Item.AddToDatabase(item, price))
                    await ReplyAsync("El objeto fue añadido exitosamente a la base de datos");
                else
                    await ReplyAsync("No se pudo añadir el objeto a la base de datos");
            }
            else
                await ReplyAsync("No se poseen permisos para realizar esta operacion");
        }
    }
}
