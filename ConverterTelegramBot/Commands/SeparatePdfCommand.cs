using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using FileHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Commands;

public class SeparatePdfCommand : ICommand
{
    private readonly IUserService _userService;

    private readonly IPdfConverter _pdfConverter;

    private readonly IChatDataProvider _chatDataProvider;

    public SeparatePdfCommand(
        IUserService userService,
        IPdfConverter pdfConverter,
        IChatDataProvider chatDataProvider
    )
    {
        _userService = userService;
        _pdfConverter = pdfConverter;
        _chatDataProvider = chatDataProvider;
    }

    public string Name => CommandName.SeparateCommandName;

    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);

        var range = GetPagesRange(update.Message.Text);
        var fileBytes = Convert.FromBase64String(user.LastDocument);
        byte[] separatedDocumentBytes = new byte[0];

        try
        {
            separatedDocumentBytes = _pdfConverter.SeparateFile(
                fileBytes,
                range.First(),
                range.Last()
            );
        }
        catch
        {
            _chatDataProvider.SendMessage(user.ChatId, "Ошибка разделения файла");
        }

        _chatDataProvider.SendPdfFile(user.ChatId, separatedDocumentBytes);
    }

    private List<int> GetPagesRange(string rangeMessage)
    {
        var textRange = rangeMessage.Split(' ');
        var range = textRange.Select(int.Parse).ToList();
        return range;
    }
}
