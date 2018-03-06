using System;
using System.Configuration;
using System.Threading.Tasks;
using Discord.Commands;


namespace DnDSalesBot.CommandModules
{
	class HelpModule : ModuleBase
	{
		#region Privates
		private CommandService _service;
	
		private async void InitializeService()
		{
			_service = new CommandService();
			await _service.AddModuleAsync<BuyModule>();
			await _service.AddModuleAsync<SellModule>();
			await _service.AddModuleAsync<AddItemModule>();
			await _service.AddModuleAsync<AddPlayerModule>();
			await _service.AddModuleAsync<HelpModule>();
			await _service.AddModuleAsync<MakeDmModule>();
			await _service.AddModuleAsync<GiveGoldModule>();
		}
		#endregion

		#region Commands
		[Command("Help"), Summary("Muestra el texto de ayuda para todos los comandos")]
		public async Task Help()
		{
			InitializeService();

			String answer = "```Comandos:\n";

			foreach (ModuleInfo Module in _service.Modules)
			{
				foreach (CommandInfo cmd in Module.Commands)
				{
					answer += ConfigurationManager.AppSettings["commandPrefix"] + cmd.Name;
					foreach (ParameterInfo parameter in cmd.Parameters)
						answer += " <" + parameter.Name + ">";
					answer += ": " + cmd.Summary + "\n";
				}
			}
			answer += "```";

			await ReplyAsync(answer);
			
		}

		[Command("Help"), Summary("Muestra el texto de ayuda para el comando especificado")]
		public async Task Help([Remainder]string commandHelp)
		{
			commandHelp.ToLower();
			InitializeService();

			SearchResult result = _service.Search(Context, commandHelp);

			if (!result.IsSuccess)
				await ReplyAsync("Comando no encontrado");
			else
			{
				string answer = "```Comandos:\n";

				foreach (CommandMatch match in result.Commands)
				{
					answer += ConfigurationManager.AppSettings["commandPrefix"] + match.Command.Name;
					foreach (ParameterInfo parameter in match.Command.Parameters)
						answer += " <" + parameter.Name + ">";
					answer += ": " + match.Command.Summary + "\n";
				}
				answer += "```";

				await ReplyAsync(answer);
			}
			
		}
		#endregion
	}
}
