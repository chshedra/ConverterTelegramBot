using System.IO;
using Aspose;
using Aspose.Pdf;
using Aspose.Pdf.Text;

namespace FileHandler
{
	public class PdfHandler
	{
		public byte[] ConvertToPdf(string text)
		{
			var document = new Document();

			var page = document.Pages.Add();

			page.Paragraphs.Add(new TextFragment(text));

			var memoryStream = new MemoryStream();

			document.Save(memoryStream, SaveFormat.Pdf);

			return memoryStream.ToArray();
		}
	}
}