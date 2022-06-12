using System.Threading.Tasks;
using ConverterTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConverterTelegramBot.Services
{
	public class UserService : IUserService
	{
		private readonly BotDbContext _context;

		public UserService(BotDbContext context)
		{
			_context = context;
		}

		public async Task<BotUserEntity> GetUser(Update update)
		{
			var newUser = update.Type switch
			{
				UpdateType.CallbackQuery => new BotUserEntity
				{
					UserName = update.CallbackQuery.From.Username,
					ChatId = update.CallbackQuery.Message.Chat.Id,
					FirstName = update.CallbackQuery.Message.From.FirstName,
					LastName = update.CallbackQuery.Message.From.LastName
				},
				UpdateType.Message => new BotUserEntity
				{
					UserName = update.Message.Chat.Username,
					ChatId = update.Message.Chat.Id,
					FirstName = update.Message.Chat.FirstName,
					LastName = update.Message.Chat.LastName
				}
			};

			var user = 
				await _context.Users.FirstOrDefaultAsync(x => x.ChatId == newUser.ChatId);

			if (user != null)
			{
				return user;
			}

			var result = await _context.Users.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return result.Entity;
		}
	}
}