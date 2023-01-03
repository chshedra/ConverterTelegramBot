using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using System.IO;
using System.Text;

namespace FileHandler;

/// <summary>
/// Convert input data to pdf
/// </summary>
public class PdfConverter
{
    /// <summary>
    /// Convert text to pdf bytes
    /// </summary>
    /// <param name="text">Input text</param>
    /// <returns>Pdf bytes</returns>
    public static byte[] ConvertToPdf(string text)
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
}
