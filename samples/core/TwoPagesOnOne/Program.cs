using System;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace TwoPagesOnOne
{
    /// <summary>
    /// This sample shows how to place two pages of an existing document on
    /// one landscape orientated page of a new document.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Get a fresh copy of the sample PDF file.
            var filename = "Portable Document Format.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), filename);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename), file, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

            // Create the output document.
            var outputDocument = new PdfDocument();

            // Show single pages.
            // (Note: one page contains two pages from the source document)
            outputDocument.PageLayout = PdfPageLayout.SinglePage;

            var font = new XFont("Verdana", 8, XFontStyle.Bold);
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Center;
            format.LineAlignment = XLineAlignment.Far;

            // Open the external document as XPdfForm object.
            var form = XPdfForm.FromFile(filename);

            for (var idx = 0; idx < form.PageCount; idx += 2)
            {
                // Add a new page to the output document.
                var page = outputDocument.AddPage();
                page.Orientation = PageOrientation.Landscape;
                double width = page.Width;
                double height = page.Height;

                var gfx = XGraphics.FromPdfPage(page);

                // Set the page number (which is one-based).
                form.PageNumber = idx + 1;

                var box = new XRect(0, 0, width / 2, height);
                // Draw the page identified by the page number like an image.
                gfx.DrawImage(form, box);

                // Write page number on each page.
                box.Inflate(0, -10);
                gfx.DrawString(String.Format("- {0} -", idx + 1),
                    font, XBrushes.Red, box, format);

                if (idx + 1 >= form.PageCount)
                    continue;

                // Set the page number (which is one-based).
                form.PageNumber = idx + 2;

                box = new XRect(width / 2, 0, width / 2, height);
                // Draw the page identified by the page number like an image.
                gfx.DrawImage(form, box);

                // Write page number on each page.
                box.Inflate(0, -10);
                gfx.DrawString(String.Format("- {0} -", idx + 2),
                    font, XBrushes.Red, box, format);
            }

            // Save the document...
            filename = "TwoPagesOnOne_tempfile.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}
