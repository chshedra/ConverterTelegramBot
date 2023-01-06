namespace FileHandler
{
    public interface IPdfConverter
    {
        byte[] ConvertToPdf(string text);

        byte[] SeparateFile(byte[] defaultFileBytes, int fromPage, int toPage);
    }
}
