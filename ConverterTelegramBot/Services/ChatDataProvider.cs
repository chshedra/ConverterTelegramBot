using FileHandler;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace ConverterTelegramBot.Services;

/// <inheritdoc/>
public class ChatDataProvider : IChatDataProvider
{
    private readonly IPdfConverter _pdfConverter;

    public ChatDataProvider(IPdfConverter pdfConverter)
    {
        _pdfConverter = pdfConverter;
    }

    public async void SendMessage(TelegramBotClient botClient, long chatId, string message)
    {
        await botClient.SendTextMessageAsync(chatId, message);
    }

    /// <inheritdoc/>
    public async void SendPdfFile(TelegramBotClient botClient, long chatId, byte[] fileBytes)
    {
        using (var stream = new FileStream("temp.pdf", FileMode.Create))
        {
            foreach (var fileByte in fileBytes)
            {
                stream.WriteByte(fileByte);
            }

            stream.Seek(0, SeekOrigin.Begin);
            var document = new InputOnlineFile(stream, "file.pdf");
            await botClient.SendDocumentAsync(chatId, document);
        }
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetPdfBytes(string text) =>
        await Task.FromResult(_pdfConverter.ConvertToPdf(text));
}
