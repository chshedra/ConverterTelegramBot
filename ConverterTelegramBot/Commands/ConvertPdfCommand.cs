using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Commands;

/// <summary>
/// Converting to PDF command
/// </summary>
public class ConvertPdfCommand : ICommand
{
    private readonly IUserService _userService;

    private readonly TelegramBotClient _botClient;

    private readonly IChatDataProvider _dataProvider;

    /// <inheritdoc/>
    public string Name => CommandName.ConvertPdfCommandName;

    /// <summary>
    /// Create instance of converting to PDF command
    /// </summary>
    /// <param name="userService">Getting user info service</param>
    /// <param name="bot">Getting bot client service</param>
    /// <param name="dataProvider">Getting chat data service</param>
    public ConvertPdfCommand(IUserService userService, Bot bot, IChatDataProvider dataProvider)
    {
        _userService = userService;
        _botClient = bot.GetBot().Result;
        _dataProvider = dataProvider;
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);
        byte[] fileBytes = default;

        switch (update.Message?.Type)
        {
            case MessageType.Text:
            {
                break;
            }
            default:
            {
                _dataProvider.SendMessage(user.ChatId, CommandMessage.UnsupportedFileMessage);
                break;
            }
        }

        _dataProvider.SendPdfFile(user.ChatId, fileBytes);
    }

    private string GetFileID(Update update) =>
        update.Message?.Photo[update.Message.Photo.Length - 1].FileId;
}
