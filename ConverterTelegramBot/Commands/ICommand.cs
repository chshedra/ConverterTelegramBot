using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Commands;

/// <summary>
/// Defining command behavior
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Command name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Execute command async
    /// </summary>
    /// <param name="update">Updated data from client </param>
    Task ExecuteAsync(Update update);
}
