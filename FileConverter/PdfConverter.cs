using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Data;
using System.IO;
using System.Linq;

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

    public byte[] SeparateFile(byte[] defaultFileBytes, int fromPage, int toPage)
    {
        PdfDocument document = new PdfDocument();

        using (var stream = new MemoryStream(defaultFileBytes))
        {
            document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        }

        var separatedDocument = new PdfDocument();
        var startPageIndex = fromPage - 1;
        var pageCount = toPage - fromPage + 1;
        separatedDocument.Pages.InsertRange(0, document, startPageIndex, pageCount);

        byte[] separatedDocumentBytes = new byte[0];
        using (var stream = new MemoryStream())
        {
            separatedDocument.Save(stream);
            separatedDocumentBytes = stream.ToArray();
        }

        return separatedDocumentBytes;
    }
}
