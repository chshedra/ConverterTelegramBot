using ConverterTelegramBot.Models;
using FileHandler;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace ConverterTelegramBot.Services;

/// <inheritdoc/>
public class ChatDataProvider : IChatDataProvider
{
    private readonly IPdfConverter _pdfConverter;

    private readonly TelegramBotClient _botClient;

    private readonly BotDbContext _dbContext;

    public ChatDataProvider(IPdfConverter pdfConverter, Bot bot, BotDbContext dbContext)
    {
        _pdfConverter = pdfConverter;
        _dbContext = dbContext;
        _botClient = bot.GetBot().Result;
    }

    public async void SendMessage(long chatId, string message)
    {
        await _botClient.SendTextMessageAsync(chatId, message);
    }

    /// <inheritdoc/>
    public async void SendPdfFile(long chatId, byte[] fileBytes)
    {
        using (var stream = new FileStream("temp.pdf", FileMode.Create))
        {
            foreach (var fileByte in fileBytes)
            {
                stream.WriteByte(fileByte);
            }

            stream.Seek(0, SeekOrigin.Begin);
            var document = new InputOnlineFile(stream, "file.pdf");
            await _botClient.SendDocumentAsync(chatId, document);
        }
    }

    public async Task SaveFileFromChat(string fileId, long chatId)
    {
        var file = await _botClient.GetFileAsync(fileId);

        byte[] fileBytes = new byte[0];
        using (var stream = new MemoryStream())
        {
            await _botClient.DownloadFileAsync(file.FilePath, stream);
            fileBytes = stream.ToArray();
        }

        var user = await _dbContext.Users.FirstAsync(u => u.ChatId == chatId);
        user.LastDocument = Convert.ToBase64String(fileBytes);
        _dbContext.Update(user);

        _dbContext.SaveChanges();
    }
}
