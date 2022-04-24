using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ConverterTelegramBot.Models
{
	public class Bot
	{
		private readonly IConfiguration _configuration;

		private TelegramBotClient _botClient;

		public Bot(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<TelegramBotClient> GetBot()
		{
			if (_botClient != null)
			{
				return _botClient;
			}

			_botClient = new TelegramBotClient(_configuration["Token"]);

			var hook = $"{_configuration["Url"]}api/message/update";
			await _botClient.SetWebhookAsync(hook);

			return _botClient;

		}
	}
}