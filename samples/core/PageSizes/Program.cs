using System;
using System.Diagnostics;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PageSizes
{
    /// <summary>
    /// This sample shows a document with different page sizes.
    /// Note: You can set the size of a page to any size using the Width and
    /// Height properties. This sample just shows the predefined sizes.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Create a new PDF document.
            var document = new PdfDocument();

            // Create a font.
            var font = new XFont("Times New Roman", 25, XFontStyle.Bold);

            var pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            foreach (var pageSize in pageSizes)
            {
                if (pageSize == PageSize.Undefined)
                    continue;

                // One page in Portrait...
                var page = document.AddPage();
                page.Size = pageSize;
                var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize.ToString(), font, XBrushes.DarkRed,
                    new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.Center);

                // ... and one in Landscape orientation.
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize + " (landscape)", font,
                    XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.Center);
            }

            // Save the document...
            const string filename = "PageSizes_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}