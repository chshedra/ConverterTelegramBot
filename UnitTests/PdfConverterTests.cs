using NUnit.Framework;
using System.Collections;
using FileHandler;
using System.IO;
using System.Text;
using System;

namespace UnitTests
{
    [TestFixture]
    internal class PdfConverterTests
    {
        [TestCaseSource(nameof(ConvertTextData))]
        public void ConvertTextToPdfTest(string inputText, byte[] expectedBytes)
        {
            //Arrange & Act
            byte[] actualBytes = PdfConverter.ConvertToPdf(inputText);

            //Assert
            Assert.IsNotNull(actualBytes);
            Assert.AreEqual(expectedBytes.Length, actualBytes.Length);
        }

        private static IEnumerable ConvertTextData
        {
            get
            {
                yield return new TestCaseData(
                    "Тетстовый текст",
                    TestManager.GetBytes(TestManager.ExpectedCyrillicPdfPath)
                );
                yield return new TestCaseData(
                    "TestText",
                    TestManager.GetBytes(TestManager.ExpectedLatinPdfPath)
                );
                yield return new TestCaseData(
                    string.Empty,
                    TestManager.GetBytes(TestManager.ExpectedEmptyPdfPath)
                );
            }
        }

        private static IEnumerable ConvertImageData
        {
            get
            {
                yield return new TestCaseData(
                    TestManager.GetBytes(TestManager.SourceTestImagePath),
                    TestManager.GetBytes(TestManager.ExpectedImageBytesPath)
                );
            }
        }
    }
}
