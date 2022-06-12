using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Services
{
	public interface IUserService
	{
		Task<BotUserEntity> GetUser(Update update);
	}
}