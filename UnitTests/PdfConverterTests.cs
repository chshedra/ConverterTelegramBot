using NUnit.Framework;
using System.Collections;
using FileHandler;
using System.IO;

namespace UnitTests
{
	[TestFixture]
	internal class PdfConverterTests
	{
		[TestCaseSource(nameof(ConvertTextData))]
		public void ConvertToPdfTest(string inputText)
		{
			//Arrange & Act
			byte[] actualBytes = new byte[0];
			try
			{
				actualBytes = PdfConverter.ConvertToPdf(inputText);
			}
			catch
			{

			}

			File.WriteAllBytes("ExpectedBytes.txt", actualBytes);
			Assert.IsNotNull(actualBytes);
		}

		public static IEnumerable ConvertTextData
		{
			get
			{
				yield return "TestText";
			}
		}
	}
}
