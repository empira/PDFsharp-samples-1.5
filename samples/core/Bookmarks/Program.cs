using System.Diagnostics;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Bookmarks
{
    /// <summary>
    /// This sample shows how to create bookmarks. Bookmarks are called outlines
    /// in the PDF reference manual, that's why you deal with the class PdfOutline.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Create a new PDF document.
            var document = new PdfDocument();

            // Create a font.
            var font = new XFont("Verdana", 16);

            // Create the first page.
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString("Page 1", font, XBrushes.Black, 20, 50, XStringFormats.Default);

            // Create the root bookmark. You can set the style and the color.
            var outline = document.Outlines.Add("Root", page, true, PdfOutlineStyle.Bold, XColors.Red);

            // Create some more pages.
            for (var idx = 2; idx <= 5; idx++)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);

                var text = "Page " + idx;
                gfx.DrawString(text, font, XBrushes.Black, 20, 50, XStringFormats.Default);

                // Create a sub bookmark.
                outline.Outlines.Add(text, page, true);
            }

            // Save the document...
            const string filename = "Bookmarks_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}