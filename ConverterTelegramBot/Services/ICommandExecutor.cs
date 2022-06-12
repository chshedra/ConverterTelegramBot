using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Services
{
	/// <summary>
	/// Поведение классов выполнения команд.
	/// </summary>
	public interface ICommandExecutor
	{
		/// <summary>
		/// Выполняет команду.
		/// </summary>
		Task Execute(Update update);
	}
}