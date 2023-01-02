using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ConverterTelegramBot.Models;

/// <summary>
/// Telegram bot info
/// </summary>
public class Bot
{
    /// <summary>
    /// Application configuration properties
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Client for Telegram Bot API
    /// </summary>
    private TelegramBotClient _botClient;

    /// <summary>
    /// Create bot instance by configuration
    /// </summary>
    public Bot(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Get client for Telegram Bot API
    /// </summary>
    public async Task<TelegramBotClient> GetBot()
    {
        if (_botClient != null)
        {
            return _botClient;
        }

        //get bot token from config file
        _botClient = new TelegramBotClient(_configuration["Token"]);

        //set web hook for new messages from user
        var hook = $"{_configuration["Url"]}api/message/update";
        await _botClient.SetWebhookAsync(hook);

        return _botClient;
    }
}
