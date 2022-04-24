using System;

namespace AssistantBot.Models
{
	public class BaseEntity
	{
		public long Id { get; set; }
		
		public DateTime Created { get; set;  } = DateTime.UtcNow;
	}
}