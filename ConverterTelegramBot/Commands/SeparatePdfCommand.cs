using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using FileHandler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Commands;

public class SeparatePdfCommand : ICommand
{
    private readonly IUserService _userService;

    private readonly TelegramBotClient _botClient;

    private readonly IPdfConverter _pdfConverter;

    private readonly IChatDataProvider _chatDataProvider;

    public SeparatePdfCommand(
        IUserService userService,
        Bot bot,
        IPdfConverter pdfConverter,
        IChatDataProvider chatDataProvider
    )
    {
        _userService = userService;
        _botClient = bot.GetBot().Result;
        _pdfConverter = pdfConverter;
        _chatDataProvider = chatDataProvider;
    }

    public string Name => "SeparateCommand";

    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);

        var fileId = update.Message?.Document.FileId;
        var fileInfo = await _botClient.GetFileAsync(fileId);

        byte[] fileBytes;
        using (var stream = new MemoryStream())
        {
            await _botClient.DownloadFileAsync(fileInfo.FilePath, stream);
            fileBytes = stream.ToArray();
        }

        var range = GetPagesRange(update.Message.Text);
        var separatedDocumentBytes = _pdfConverter.SeparateFile(
            fileBytes,
            range.First(),
            range.Last()
        );
        _chatDataProvider.SendPdfFile(_botClient, user.ChatId, separatedDocumentBytes);
    }

    private List<int> GetPagesRange(string rangeMessage)
    {
        var textRange = rangeMessage.Split(' ');
        var range = textRange.Select(r => int.Parse(r)).ToList();
        return range;
    }
}
