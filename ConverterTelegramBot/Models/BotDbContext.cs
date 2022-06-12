using Microsoft.EntityFrameworkCore;

namespace ConverterTelegramBot.Models
{
	/// <summary>
	/// Bot database context
	/// </summary>
	public class BotDbContext : DbContext
	{
		public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

		public DbSet<BotUserEntity> Users { get; set; }

		public DbSet<FileEntity> FileEntities { get; set; }

	}
}