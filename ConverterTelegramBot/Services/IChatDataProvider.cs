using System.Threading.Tasks;
using Telegram.Bot;

namespace ConverterTelegramBot.Services
{
	/// <summary>
	/// Get and send data from chat
	/// </summary>
	public interface IChatDataProvider
	{
		/// <summary>
		/// Send pdf file to user
		/// </summary>
		void SendPdfFile(TelegramBotClient botClient, long chatId, byte[] fileBytes);

		/// <summary>
		/// Get bytes of converted image pdf
		/// </summary>
		/// <returns>Bytes of pdf file</returns>
		Task<byte[]> GetPdfBytes(TelegramBotClient botClient, string fileId);

		/// <summary>
		/// Get bytes of converted string pdf
		/// </summary>
		/// <returns>Bytes of pdf file</returns>
		Task<byte[]> GetPdfBytes(string text);
	}
}