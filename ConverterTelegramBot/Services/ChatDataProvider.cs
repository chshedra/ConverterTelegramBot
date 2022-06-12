using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace ConverterTelegramBot.Services
{
	public class ChatDataProvider : IChatDataProvider
	{
		public async void SendPdfFile(TelegramBotClient botClient, long chatId, byte[] fileBytes)
		{
			using (var fs = new FileStream("temp.pdf", FileMode.Create))
			{
				foreach (var bt in fileBytes)
				{
					fs.WriteByte(bt);
				}

				fs.Seek(0, SeekOrigin.Begin);
				var document = new InputOnlineFile(fs, "file.pdf");
				await botClient.SendDocumentAsync(chatId, document);
			}
		}

		public async Task<byte[]> GetImageBytes(TelegramBotClient botClient, Telegram.Bot.Types.File file)
		{
			byte[] fileBytes;
			MemoryStream memoryStream;

			using (memoryStream = new MemoryStream())
			{
				await botClient.DownloadFileAsync(file.FilePath, memoryStream);
				fileBytes = FileHandler.PdfConverter.ConvertToPdf(memoryStream);
			}

			return fileBytes;
		}
	}
}