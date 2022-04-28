using System;

namespace ConverterTelegramBot.Models
{
	public class BaseEntity
	{
		public long Id { get; set; }
		
		public DateTime Created { get; set;  } = DateTime.UtcNow;
	}
}