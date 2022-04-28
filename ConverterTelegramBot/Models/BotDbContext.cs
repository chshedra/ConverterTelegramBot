using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ConverterTelegramBot.Models
{
	public class BotDbContext : DbContext
	{
		public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

		public DbSet<BotUserEntity> Users { get; set; }

		public DbSet<FileEntity> FileEntities { get; set; }

	}
}