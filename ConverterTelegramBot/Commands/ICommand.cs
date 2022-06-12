using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Commands
{
	public interface ICommand
	{
		string Name { get; }

		Task ExecuteAsync(Update update);
	}
}