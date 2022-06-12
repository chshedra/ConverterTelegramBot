using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Services
{
	/// <summary>
	/// Service for working with user
	/// </summary>
	public interface IUserService
	{
		/// <summary>
		/// Get char user
		/// </summary>
		/// <param name="update">New messages values</param>
		/// <returns>Bot user entity</returns>
		Task<BotUserEntity> GetUser(Update update);
	}
}