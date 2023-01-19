using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using FileHandler;
using System;
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

    public string Name => CommandName.SeparateCommandName;

    public async Task ExecuteAsync(Update update)
    {
        var user = await _userService.GetUser(update);

        var range = GetPagesRange(update.Message.Text);
        var fileBytes = Convert.FromBase64String(user.LastDocument);
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
        var range = textRange.Select(int.Parse).ToList();
        return range;
    }
}
