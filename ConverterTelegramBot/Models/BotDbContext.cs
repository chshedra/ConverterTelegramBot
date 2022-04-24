using AssistantBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ConverterTelegramBot.Models
{
	public class BotDbContext : DbContext
	{
		public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
		{

		}

		public DbSet<BotUser> Users { get; set; }

	}
}