using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConverterTelegramBot.Commands
{
	/// <summary>
	/// Starting chat command
	/// </summary>
	public class StartCommand : ICommand
	{

		private readonly IUserService _userService;

		private readonly TelegramBotClient _botClient;

		/// <inheritdoc/>
		public string Name => "StartCommand";

		/// <summary>
		/// Create instance of start command
		/// </summary>
		/// <param name="userService">Getting user data service</param>
		/// <param name="bot">Telegram bot info</param>
		public StartCommand(IUserService userService, Bot bot)
		{
			_userService = userService;
			_botClient = bot.GetBot().Result;
		}

		/// <inheritdoc/>
		public async Task ExecuteAsync(Update update)
		{
			var user = await _userService.GetUser(update);

			var inlineKeyboard = new ReplyKeyboardMarkup(new[]
			{
				new[]
				{
					new KeyboardButton("Конвертировать текст в PDF")
				}
			});

			await _botClient.SendTextMessageAsync(user.ChatId, "Выберите необходимое действие",
				ParseMode.Markdown, replyMarkup: inlineKeyboard);
		}
	}
}