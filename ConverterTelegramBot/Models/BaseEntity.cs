using System;

namespace ConverterTelegramBot.Models
{
	/// <summary>
	/// Base entity for database entities
	/// </summary>
	public class BaseEntity
	{
		/// <summary>
		/// Entity ID
		/// </summary>
		public long Id { get; set; }
		
		/// <summary>
		/// Entity creation date
		/// </summary>
		public DateTime Created { get; set;  } = DateTime.UtcNow;
	}
}