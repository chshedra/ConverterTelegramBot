using System.Drawing;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace FileHandler
{
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
			var stream = new MemoryStream();
			var document = new Document();
			Section section = document.AddSection();
			var paragraph = new Paragraph();
			paragraph.AddText(text);
			section.Add(paragraph);

			var pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
			pdfRenderer.Document = document;
			pdfRenderer.RenderDocument();
			pdfRenderer.PdfDocument.Save(stream);

			return stream.ToArray();
		}

		/// <summary>
		/// Convert image to pdf bytes
		/// </summary>
		/// <param name="imageStream">Image stream</param>
		/// <returns>Pdf bytes</returns>
		public static byte[] ConvertToPdf(MemoryStream imageStream)
		{
			var document = new PdfDocument();
			PdfPage page = document.AddPage();

			XGraphics drawSurface = XGraphics.FromPdfPage(page);

			DrawImage(drawSurface, imageStream, page.Width.Point, page.Height.Point);

			var pdfStream = new MemoryStream();
			document.Save(pdfStream);

			return pdfStream.ToArray();
		}

		/// <summary>
		/// Draw image on surface
		/// </summary>
		/// <param name="drawSurface">Drawing surface</param>
		/// <param name="stream">Stream with image</param>
		/// <param name="x">XCoordinate</param>
		/// <param name="y">YCoordinate</param>
		private static void DrawImage(XGraphics drawSurface, MemoryStream stream, 
			double width, double height)
		{
			var image = XImage.FromStream(stream);

			drawSurface.DrawImage(image, 0, 0, width, height);
		}
	}
}