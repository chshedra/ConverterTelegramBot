using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Controllers
{ 
	[ApiController]
	[Route("api/message")]
	public class BotController : ControllerBase
	{
		private readonly TelegramBotClient _telegramBotClient;

		private readonly BotDbContext _botDbContext;

		public BotController(Bot telegramBot, BotDbContext dbContext)
		{
			_botDbContext = dbContext;
			_telegramBotClient = telegramBot.GetBot().Result;
		}

		[HttpPost]
		[Route("update")]
		public async Task<IActionResult> Update([FromBody]object update)
		{
			var upd = JsonConvert.DeserializeObject<Update>(update.ToString());

			var chat = upd.Message?.Chat;

			if (chat == null)
			{
				return Ok();
			}

			var botUser = new BotUserEntity()
			{
				UserName = chat.Username,
				ChatId = chat.Id,
				FirstName = chat.FirstName,
				LastName = chat.LastName
			};

			var result = await _botDbContext.Users.AddAsync(botUser);
			await _botDbContext.SaveChangesAsync();

			_telegramBotClient.SendTextMessageAsync(chat.Id,
				"Вы успешно зарегестрировались", ParseMode.Markdown);

			return Ok();
		}
	}
}
