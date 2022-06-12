using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Pdf;
using Aspose.Pdf.Devices;
using Image = System.Drawing.Image;

namespace TestFileHandler
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CreatePDF();
		}

		private static void CreatePDF()
		{
			var image = Image.FromFile(@"C:\Users\Andrey\Desktop\Универ\AAA.jpg");

			MemoryStream m = new MemoryStream();
			image.Save(m, image.RawFormat);
			

			var asposePdf = new Aspose.Pdf.Image();
			asposePdf.ImageStream = m;

			Document pdfDocument = new Document();
			var page = pdfDocument.Pages.Add();
			page.Paragraphs.Add(asposePdf);

			var memoryStream = new MemoryStream();
			pdfDocument.Save(memoryStream);

			var bytes = memoryStream.ToArray();

			using (var fs = new FileStream(@"C:\Users\Andrey\Desktop\Универ\FileName.pdf", FileMode.Create))
			{
				foreach (var bt in bytes)
				{
					fs.WriteByte(bt);
				}

				fs.Seek(0, SeekOrigin.Begin);
			}
		}

		
	}
}
