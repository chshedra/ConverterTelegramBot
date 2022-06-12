using System.Threading.Tasks;
using Telegram.Bot;

namespace ConverterTelegramBot.Services
{
	public interface IChatDataProvider
	{
		void SendPdfFile(TelegramBotClient botClient, long chatId, byte[] fileBytes);

		Task<byte[]> GetImageBytes(TelegramBotClient botClient, Telegram.Bot.Types.File file);
	}
}