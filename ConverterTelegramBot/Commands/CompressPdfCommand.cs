using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using FileHandler;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ConverterTelegramBot.Commands
{
    public class CompressPdfCommand : ICommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;
        private readonly IPdfConverter _pdfConverter;
        private readonly IChatDataProvider _dataProvider;

        public string Name => CommandName.CompressPdfCommand;

        public CompressPdfCommand(
            IUserService userService,
            Bot bot,
            IPdfConverter pdfConverter,
            IChatDataProvider dataProvider
        )
        {
            _userService = userService;
            _pdfConverter = pdfConverter;
            _botClient = bot.GetBot().Result;
            _dataProvider = dataProvider;
        }

        public async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetUser(update);

            var file = await _botClient.GetFileAsync(update.Message?.Document?.FileId);

            byte[] fileBytes = new byte[0];
            using (var stream = new MemoryStream())
            {
                await _botClient.DownloadFileAsync(file.FilePath, stream);
                fileBytes = stream.ToArray();
            }

            var compressedPdf = _pdfConverter.CompressPdf(fileBytes);
            _dataProvider.SendPdfFile(user.ChatId, compressedPdf);
        }
    }
}
