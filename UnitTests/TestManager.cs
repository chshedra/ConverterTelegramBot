using System.IO;
using System.Reflection;

namespace UnitTests
{
	internal static class TestManager
	{
		internal static string ExpectedLatinPdfPath => "TestData\\ExpectedLatinBytes.txt";

		internal static string ExpectedCyrillicPdfPath => "TestData\\ExpectedCyrillicBytes.txt";

		internal static string ExpectedEmptyPdfPath => "TestData\\ExpectedEmptyBytes.txt";

		internal static string ExpectedImageBytesPath => "TestData\\ExpectedImageBytes.txt";

		internal static string SourceTestImagePath => "TestData\\TestImage.png";

		internal static byte[] GetBytes(string fileName)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(assembly.Location);
			var resultPath = Path.Combine(path, fileName);

			return File.ReadAllBytes(resultPath);
		}


	}
}
