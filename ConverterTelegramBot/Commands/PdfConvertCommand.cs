using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace ConverterTelegramBot.Commands
{
	public class PdfConvertCommand : ICommand
	{
		private readonly BotDbContext _context;

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
					fileBytes = FileHandler.PdfConverter.ConvertToPdf(update.Message?.Text);
					break;
				}
				case MessageType.Photo:
				{
					var fileId = update.Message?.Photo[update.Message.Photo.Length - 1].FileId;
					
					var file = await _botClient.GetFileAsync(fileId);

					fileBytes = _dataProvider.GetImageBytes(_botClient, file).Result;
					
					break;
				}
			}
			
			_dataProvider.SendPdfFile(_botClient, user.ChatId, fileBytes);
		}
	}
}