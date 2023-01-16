using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Commands;

public class GetTextCommand : ICommand
{
    private readonly TelegramBotClient _botClient;

    private readonly IUserService _userService;
    public string Name => CommandName.GetTextCommandName;

    public GetTextCommand(IUserService userService, Bot bot)
    {
        _userService = userService;
        _botClient = bot.GetBot().Result;
    }

    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);

        await _botClient.SendTextMessageAsync(
            user.ChatId,
            "Отправьте текст или изображение для конвертации",
            ParseMode.Markdown
        );
    }
}
