namespace FileHandler
{
    public interface IPdfConverter
    {
        byte[] ConvertToPdf(string text);
    }
}