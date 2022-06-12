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

		public string Name => "PdfConvertCommand";

		public PdfConvertCommand(IUserService userService, Bot bot)
		{
			_userService = userService;
			_botClient = bot.GetBot().Result;
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

					MemoryStream memoryStream;
					using (memoryStream = new MemoryStream())
					{
						await _botClient.DownloadFileAsync(file.FilePath, memoryStream);
						var msBytes = memoryStream.ToArray();
						fileBytes = FileHandler.PdfConverter.ConvertToPdf(memoryStream);
					}

					break;
				}
			}
			
			using (var fs = new FileStream("FileName.pdf", FileMode.Create))
			{
				foreach (var bt in fileBytes)
				{
					fs.WriteByte(bt);
				}

				fs.Seek(0, SeekOrigin.Begin);
				var document = new InputOnlineFile(fs, "file.pdf");
				await _botClient.SendDocumentAsync(user.ChatId, document);
			}
		}
	}
}