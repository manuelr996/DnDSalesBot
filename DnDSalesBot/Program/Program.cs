using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using DnDSalesBot.CommandModules;
using Discord.WebSocket;


namespace DnDSalesBot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();



        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            string token = ConfigurationManager.AppSettings["botToken"];

            services = new ServiceCollection().BuildServiceProvider();

            try
            {
                await InstallCommands();
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception Caught: " + e.Message);
            }

            await Task.Delay(-1);
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            SocketUserMessage msg = message as SocketUserMessage;
            char commandPrefix = ConfigurationManager.AppSettings["commandPrefix"][0];
            int argPos = 0;

            if (msg != null)
            {
				LogMessage log;
				if ((msg.HasCharPrefix(commandPrefix, ref argPos))) // || msg.HasMentionPrefix(client.CurrentUser, ref argPos)
				{
                    CommandContext context = new CommandContext(client, msg);
					IResult result = await commands.ExecuteAsync(context, argPos, services);
					if (!result.IsSuccess)
                    {
                        log = new LogMessage(LogSeverity.Error, result.Error.Value.ToString(), result.ErrorReason);

                        switch(result.Error.Value)
                        {
                            case CommandError.BadArgCount:
                                await message.Channel.SendMessageAsync("Argument amount is wrong");
                                break;
                            case CommandError.UnknownCommand:
                                await message.Channel.SendMessageAsync("Command unknown refer to !help");
                                break;
                        }
                       
                    }
                    else
						log = new LogMessage(LogSeverity.Info, "Command", message.Content);

					Console.WriteLine(log.ToString());
				}
			}
        }

        private async Task InstallCommands()
        {
            client.Log += Log;
            client.MessageReceived += OnMessageReceived;
			await commands.AddModulesAsync(Assembly.GetEntryAssembly());
			await commands.AddModuleAsync<BuyModule>();
			await commands.AddModuleAsync<SellModule>();
			await commands.AddModuleAsync<AddItemModule>();
			await commands.AddModuleAsync<AddPlayerModule>();
			await commands.AddModuleAsync<HelpModule>();
			await commands.AddModuleAsync<MakeDmModule>();
			await commands.AddModuleAsync<GiveGoldModule>();
			await commands.AddModuleAsync<ConsultModule>();
			//await commands.AddModuleAsync<>();
		}
	}
}
