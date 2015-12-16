using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace SplitDocument
{
    /// <summary>
    /// This sample shows how to convert a PDF document with n pages into
    /// n documents with one page each.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Get a fresh copy of the sample PDF file
            const string filename = "Portable Document Format.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), filename);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename), file, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

            // Open the file.
            var inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import);

            var name = Path.GetFileNameWithoutExtension(filename);
            for (var idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create a new document.
                var outputDocument = new PdfDocument();
                outputDocument.Version = inputDocument.Version;
                outputDocument.Info.Title =
                    String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it.
                outputDocument.AddPage(inputDocument.Pages[idx]);
                outputDocument.Save(String.Format("{0} - Page {1}_tempfile.pdf", name, idx + 1));
            }
        }
    }
}
