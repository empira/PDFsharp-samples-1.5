using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

namespace CombineDocuments
{
    /// <summary>
    /// This sample shows how to create a new document from two existing PDF files.
    /// The pages are inserted alternately from two external documents. This may be
    /// useful for visual comparison.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // NOTE: This sample opens the same document ('Portable Document Format.pdf') twice,
            // which leads to the same page on the left and the right. This is only because I
            // don't want to add too many sample PDF files to the 'PDFs' folder and keep the 
            // download of PDFsharp small. You should replace the two files with your own stuff.

            Variant1();
            Variant2();
        }

        /// <summary>
        /// Imports pages from an external document.
        /// Note that this technique imports the whole page including the hyperlinks.
        /// </summary>
        static void Variant1()
        {
            // Get two fresh copies of the sample PDF files.
            // (Note: The input files are not modified in this sample.)
            const string filename1 = "Portable Document Format.pdf";
            var file1 = Path.Combine(Directory.GetCurrentDirectory(), filename1);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename1), file1, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file1, File.GetAttributes(file1) & ~FileAttributes.ReadOnly);


            const string filename2 = "Portable Document Format.pdf";
            var file2 = Path.Combine(Directory.GetCurrentDirectory(), filename2);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename2), file2, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file1, File.GetAttributes(file2) & ~FileAttributes.ReadOnly);

            // Open the input files.
            var inputDocument1 = PdfReader.Open(filename1, PdfDocumentOpenMode.Import);
            var inputDocument2 = PdfReader.Open(filename2, PdfDocumentOpenMode.Import);

            // Create the output document.
            var outputDocument = new PdfDocument();

            // Show consecutive pages facing. Requires Acrobat 5 or higher.
            outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;

            var font = new XFont("Verdana", 10, XFontStyle.Bold);
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Center;
            format.LineAlignment = XLineAlignment.Far;
            var count = Math.Max(inputDocument1.PageCount, inputDocument2.PageCount);
            for (var idx = 0; idx < count; idx++)
            {
                // Get the page from the 1st document.
                var page1 = inputDocument1.PageCount > idx ?
                    inputDocument1.Pages[idx] : new PdfPage();

                // Get the page from the 2nd document.
                var page2 = inputDocument2.PageCount > idx ?
                  inputDocument2.Pages[idx] : new PdfPage();

                // Add both pages to the output document.
                page1 = outputDocument.AddPage(page1);
                page2 = outputDocument.AddPage(page2);

                // Write document file name and page number on each page.
                var gfx = XGraphics.FromPdfPage(page1);
                var box = page1.MediaBox.ToXRect();
                box.Inflate(0, -10);
                gfx.DrawString(String.Format("{0} • {1}", filename1, idx + 1),
                    font, XBrushes.Red, box, format);

                gfx = XGraphics.FromPdfPage(page2);
                box = page2.MediaBox.ToXRect();
                box.Inflate(0, -10);
                gfx.DrawString(String.Format("{0} • {1}", filename2, idx + 1),
                    font, XBrushes.Red, box, format);
            }

            // Save the document...
            const string filename = "CompareDocument1_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        /// <summary>
        /// Imports the pages as form X objects.
        /// Note that this technique copies only the visual content and the
        /// hyperlinks do not work.
        /// </summary>
        static void Variant2()
        {
            // Get fresh copies of the sample PDF files.
            const string filename1 = "Portable Document Format.pdf";
            var file1 = Path.Combine(Directory.GetCurrentDirectory(), filename1);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename1), file1, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file1, File.GetAttributes(file1) & ~FileAttributes.ReadOnly);


            const string filename2 = "Portable Document Format.pdf";
            var file2 = Path.Combine(Directory.GetCurrentDirectory(), filename2);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename2), file2, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file2, File.GetAttributes(file2) & ~FileAttributes.ReadOnly);

            // Create the output document.
            var outputDocument = new PdfDocument();

            // Show consecutive pages facing.
            outputDocument.PageLayout = PdfPageLayout.TwoPageLeft;

            var font = new XFont("Verdana", 10, XFontStyle.Bold);
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Center;
            format.LineAlignment = XLineAlignment.Far;

            // Open the external documents as XPdfForm objects. Such objects are
            // treated like images. By default the first page of the document is
            // referenced by a new XPdfForm.
            var form1 = XPdfForm.FromFile(filename1);
            var form2 = XPdfForm.FromFile(filename2);

            var count = Math.Max(form1.PageCount, form2.PageCount);
            for (var idx = 0; idx < count; idx++)
            {
                // Add two new pages to the output document.
                var page1 = outputDocument.AddPage();
                var page2 = outputDocument.AddPage();

                XGraphics gfx;
                XRect box;
                if (form1.PageCount > idx)
                {
                    // Get a graphics object for page1.
                    gfx = XGraphics.FromPdfPage(page1);

                    // Set the page number (which is one-based).
                    form1.PageNumber = idx + 1;

                    // Draw the page identified by the page number like an image.
                    gfx.DrawImage(form1, new XRect(0, 0, form1.PointWidth, form1.PointHeight));

                    // Write document file name and page number on each page.
                    box = page1.MediaBox.ToXRect();
                    box.Inflate(0, -10);
                    gfx.DrawString(String.Format("{0} • {1}", filename1, idx + 1),
                        font, XBrushes.Red, box, format);
                }

                // Same as above for the second page.
                if (form2.PageCount > idx)
                {
                    gfx = XGraphics.FromPdfPage(page2);

                    form2.PageNumber = idx + 1;
                    gfx.DrawImage(form2, new XRect(0, 0, form2.PointWidth, form2.PointHeight));

                    box = page2.MediaBox.ToXRect();
                    box.Inflate(0, -10);
                    gfx.DrawString(String.Format("{0} • {1}", filename2, idx + 1),
                        font, XBrushes.Red, box, format);
                }
            }

            // Save the document...
            const string filename = "CompareDocument2_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}
