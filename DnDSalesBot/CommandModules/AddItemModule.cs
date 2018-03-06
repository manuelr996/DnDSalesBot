using System.Threading.Tasks;
using DnDSalesBot.Object_Layer;
using Discord.Commands;

namespace DnDSalesBot.CommandModules
{
    class AddItemModule : ModuleBase
    {
		#region String Macros
		private const string ITEM_ADDED = "El objeto fue añadido exitosamente a la base de datos";
		private const string ADDING_FAILED = "No se pudo añadir el objeto a la base de datos";
		#endregion

		#region Commands
		[Command("additem"),Summary("Añade un item a la base de datos (solo DMs)")]
		public async Task AddItem(string item, double price)
		{
            Player requester = Player.GetFromDatabase(Context.User.DiscriminatorValue);
			if (requester != null)
			{
				if (requester.IsDm)
				{
					if (Item.AddToDatabase(item, price))
						await ReplyAsync(ITEM_ADDED);
					else
						await ReplyAsync(ADDING_FAILED);
				}
				else
					await ReplyAsync(Utilities.INSUFFICIENT_RIGHTS);
			}
			else
				Utilities.ReportPlayerNotFound(Context, Context.User.Mention);
        }
		#endregion
	}
}
