namespace ConverterTelegramBot.Models;

public class BotUserEntity : BaseEntity
{
    public long ChatId { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string LastDocument { get; set; }
}
