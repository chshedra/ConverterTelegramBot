using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Services;

/// <summary>
/// Service for command execute
/// </summary>
public interface ICommandExecutor
{
    /// <summary>
    /// Execute command
    /// </summary>
    Task Execute(Update update);
}
