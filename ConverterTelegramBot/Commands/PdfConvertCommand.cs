using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Commands
{
	public class PdfConvertCommand : ICommand
	{
		private readonly IUserService _userService;

		private readonly TelegramBotClient _botClient;

		private readonly IChatDataProvider _dataProvider;

		public string Name => "PdfConvertCommand";

		public PdfConvertCommand(IUserService userService, Bot bot, IChatDataProvider dataProvider)
		{
			_userService = userService;
			_botClient = bot.GetBot().Result;
			_dataProvider = dataProvider;
		}

		public async Task ExecuteAsync(Update update)
		{
			var user = _userService.GetUser(update).Result;
			byte[] fileBytes = default;

			switch (update.Message?.Type)
			{
				case MessageType.Text:
				{
					fileBytes = await _dataProvider.GetPdfBytes(update.Message?.Text);
					break;
				}
				case MessageType.Photo:
				{
					var fileId = 
						update.Message?.Photo[update.Message.Photo.Length - 1].FileId;

					fileBytes = await _dataProvider.GetPdfBytes(_botClient, fileId);
					break;
				}
			}
			
			_dataProvider.SendPdfFile(_botClient, user.ChatId, fileBytes);
		}
	}
}