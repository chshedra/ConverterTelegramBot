namespace AssistantBot.Models
{
	public class BotUser : BaseEntity
	{
		public long ChatId { get; set; }

		public string UserName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}