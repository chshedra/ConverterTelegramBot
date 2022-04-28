namespace ConverterTelegramBot.Models
{
	public class FileEntity : BaseEntity
	{
		public string FileName { get; set; }

		public byte[] Data { get; set; }

		public FileType Type { get; set; }
	}
}