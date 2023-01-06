using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Commands;

public class RequestFileCommand : ICommand
{
    private readonly TelegramBotClient _botClient;

    private readonly IUserService _userService;
    public string Name => "RequestFileCommand";

    public RequestFileCommand(IUserService userService, Bot bot)
    {
        _userService = userService;
        _botClient = bot.GetBot().Result;
    }

    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);

        await _botClient.SendTextMessageAsync(user.ChatId, "Отправьте файл", ParseMode.Markdown);
    }
}
