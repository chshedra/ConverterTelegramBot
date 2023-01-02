using System.Threading.Tasks;
using ConverterTelegramBot.Infrastructure;
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
    public string Name => "PdfConvertCommand";

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
        var user = _userService.GetUser(update).Result;
        byte[] fileBytes = default;

        switch (update.Message?.Type)
        {
            case MessageType.Text:
            {
                fileBytes = await _dataProvider.GetPdfBytes(update.Message?.Text);
                break;
            }
            case MessageType.Photo:
            {
                var fileId = GetFileID(update);

                fileBytes = await _dataProvider.GetPdfBytes(_botClient, fileId);
                break;
            }
            default:
            {
                _dataProvider.SendMessage(
                    _botClient,
                    user.ChatId,
                    CommandText.UnsupportedFileMessage
                );
                break;
            }
        }

        _dataProvider.SendPdfFile(_botClient, user.ChatId, fileBytes);
    }

    private string GetFileID(Update update) =>
        update.Message?.Photo[update.Message.Photo.Length - 1].FileId;
}
