using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.IO;

namespace FileHandler;

/// <summary>
/// Convert input data to pdf
/// </summary>
public class PdfConverter : IPdfConverter
{
    /// <summary>
    /// Convert text to pdf bytes
    /// </summary>
    /// <param name="text">Input text</param>
    /// <returns>Pdf bytes</returns>
    public byte[] ConvertToPdf(string text)
    {
        var document = new PdfDocument();
        var page = document.AddPage();

        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 14, XFontStyle.Regular);

        var textColor = XBrushes.Black;
        var layout = new XRect(5, 5, page.Width, page.Height);
        var format = XStringFormats.Center;

        gfx.DrawString(text, font, textColor, layout, format);

        var stream = new MemoryStream();
        document.Save(stream);

        return stream.ToArray();
    }

    /// <summary>
    /// Separates apges range from PDF file.
    /// </summary>
    /// <param name="fileBytes">Bytes of file for searating.</param>
    /// <param name="fromPage">First page of separating range.</param>
    /// <param name="toPage">Last page of separating page.</param>
    /// <returns>Bytes of separated file.</returns>
    public byte[] SeparateFile(byte[] fileBytes, int fromPage, int toPage)
    {
        PdfDocument document = new PdfDocument();

        using (var stream = new MemoryStream(fileBytes))
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        }

        if (fromPage > toPage)
        {
            throw new FileSeparatingException(
                $"Value {nameof(fromPage)} must be less than {nameof(toPage)}"
            );
        }

        if (document.Pages.Count < toPage || fromPage < 1)
        {
            throw new FileSeparatingException("Incorrect pages range");
        }

        byte[] separatedDocumentBytes = new byte[0];
        try
        {
            var separatedDocument = new PdfDocument();
            var startPageIndex = fromPage - 1;
            var pageCount = toPage - fromPage + 1;
            separatedDocument.Pages.InsertRange(0, document, startPageIndex, pageCount);

            using (var stream = new MemoryStream())
            {
                separatedDocument.Save(stream);
                separatedDocumentBytes = stream.ToArray();
            }
        }
        catch
        {
            throw new FileSeparatingException("Unable to separate file");
        }

        return separatedDocumentBytes;
    }
}
