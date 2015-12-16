using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

namespace Watermark
{
    /// <summary>
    /// This sample shows three variations how to add a watermark text to an
    /// existing PDF file.
    /// </summary>
    class Program
    {
        static void Main()
        {
            const string watermark = "PDFsharp";
            const int emSize = 150;

            // Get a fresh copy of the sample PDF file.
            const string filename = "Portable Document Format.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), filename);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename), file, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

            // Create the font for drawing the watermark.
            var font = new XFont("Times New Roman", emSize, XFontStyle.BoldItalic);

            // Open an existing document for editing and loop through its pages.
            var document = PdfReader.Open(filename);

            // Set version to PDF 1.4 (Acrobat 5) because we use transparency.
            if (document.Version < 14)
                document.Version = 14;

            for (var idx = 0; idx < document.Pages.Count; idx++)
            {
                var page = document.Pages[idx];

                switch (idx % 3)
                {
                    case 0:
                        {
                            // Variation 1: Draw a watermark as a text string.

                            // Get an XGraphics object for drawing beneath the existing content.
                            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

                            // Get the size (in points) of the text.
                            var size = gfx.MeasureString(watermark, font);

                            // Define a rotation transformation at the center of the page.
                            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                            // Create a string format.
                            var format = new XStringFormat();
                            format.Alignment = XStringAlignment.Near;
                            format.LineAlignment = XLineAlignment.Near;

                            // Create a dimmed red brush.
                            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));

                            // Draw the string.
                            gfx.DrawString(watermark, font, brush,
                                new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                                format);
                        }
                        break;

                    case 1:
                        {
                            // Variation 2: Draw a watermark as an outlined graphical path.
                            // NYI: Does not work in Core build.

                            // Get an XGraphics object for drawing beneath the existing content.
                            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

                            // Get the size (in points) of the text.
                            var size = gfx.MeasureString(watermark, font);

                            // Define a rotation transformation at the center of the page.
                            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                            // Create a graphical path.
                            var path = new XGraphicsPath();

                            // Create a string format.
                            var format = new XStringFormat();
                            format.Alignment = XStringAlignment.Near;
                            format.LineAlignment = XLineAlignment.Near;

                            // Add the text to the path.
                            // AddString is not implemented in PDFsharp Core.
                            path.AddString(watermark, font.FontFamily, XFontStyle.BoldItalic, 150,
                            new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                                format);

                            // Create a dimmed red pen.
                            var pen = new XPen(XColor.FromArgb(128, 255, 0, 0), 2);

                            // Stroke the outline of the path.
                            gfx.DrawPath(pen, path);
                        }
                        break;

                    case 2:
                        {
                            // Variation 3: Draw a watermark as a transparent graphical path above text.
                            // NYI: Does not work in Core build.

                            // Get an XGraphics object for drawing above the existing content.
                            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);

                            // Get the size (in points) of the text.
                            var size = gfx.MeasureString(watermark, font);

                            // Define a rotation transformation at the center of the page.
                            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                            // Create a graphical path.
                            var path = new XGraphicsPath();

                            // Create a string format.
                            var format = new XStringFormat();
                            format.Alignment = XStringAlignment.Near;
                            format.LineAlignment = XLineAlignment.Near;

                            // Add the text to the path.
                            // AddString is not implemented in PDFsharp Core.
                            path.AddString(watermark, font.FontFamily, XFontStyle.BoldItalic, 150,
                                new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2),
                                format);

                            // Create a dimmed red pen and brush.
                            var pen = new XPen(XColor.FromArgb(50, 75, 0, 130), 3);
                            XBrush brush = new XSolidBrush(XColor.FromArgb(50, 106, 90, 205));

                            // Stroke the outline of the path.
                            gfx.DrawPath(pen, brush, path);
                        }
                        break;
                }
            }
            // Save the document...
            document.Save(filename);
            // ...and start a viewer
            Process.Start(filename);
        }
    }
}