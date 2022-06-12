using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConverterTelegramBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Services
{
	/// <summary>
	/// Класс управления выполнением команд.
	/// </summary>
	public class CommandExecutor : ICommandExecutor
	{
		private readonly List<ICommand> _commands;

		private ICommand _lastCommand;

		public CommandExecutor(IServiceProvider serviceProvider)
		{
			_commands = serviceProvider.GetServices<ICommand>().ToList();
		}

		/// <inheritdoc/>
		public async Task Execute(Update update)
		{
			if (update?.Message?.Chat == null && update?.CallbackQuery == null)
			{
				return;
			}

			if (update.Type == UpdateType.Message)
			{
				switch (update.Message?.Text)
				{
					case "Конвертировать текст в PDF":
					{
						await ExecuteCommand("GetTextCommand", update);
						return;
					}
				}
			}

			if (update.Message != null 
			    && update.Message.Text != null
			    && update.Message.Text.Contains("/start"))
			{
				await ExecuteCommand("StartCommand", update);
				return;
			}

			switch (_lastCommand?.Name)
			{
				case "GetTextCommand":
				{
					await ExecuteCommand("PdfConvertCommand", update);
					await ExecuteCommand("StartCommand", update);
					break;
				}
			}
		}

		private async Task ExecuteCommand(string commandName, Update update)
		{
			_lastCommand = _commands.First(x => x.Name == commandName);
			await _lastCommand.ExecuteAsync(update);
		}
	}
}