using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace FontResolver
{
    /// <summary>
    /// This sample demonstrates the use of the PDFsharp font resolver.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Register font resolver before using PDFsharp.
            // This is done globally in the current application domain.
            GlobalFontSettings.FontResolver = new SegoeWpFontResolver();
            // We use only "WinANSI" characters in this sample.
            GlobalFontSettings.DefaultFontEncoding = PdfFontEncoding.WinAnsi;

            // Create a new PDF document.
            var document = new PdfDocument();
            document.Info.Title = "Font Resolver Sample";

            // Create an empty page in this document.
            var page = document.AddPage();

            // Get an XGraphics object for drawing on this PDF page.
            var gfx = XGraphics.FromPdfPage(page);

            // Create some fonts.
            const double fontSize = 16;

            // The six font faces ordered by increasing weight.
            // See SegoeWpFontResolver for details how this works.
            var wpLightFont = new XFont("Segoe WP Light", fontSize, XFontStyle.Regular);
            var wpSemilightFont = new XFont("Segoe WP Semilight", fontSize, XFontStyle.Regular);
            var wpRegularFont = new XFont("Segoe WP", fontSize, XFontStyle.Regular);
            var wpSemiboldFont = new XFont("Segoe WP Semibold", fontSize, XFontStyle.Regular);
            var wpBoldFont = new XFont("Segoe WP", fontSize, XFontStyle.Bold);
            var wpBlackFont = new XFont("Segoe WP Black", fontSize, XFontStyle.Regular);

            // Italic simulation.
            var wpLightItalicFont = new XFont("Segoe WP Light", fontSize, XFontStyle.Italic);

            // Request of bold font face is mapped to Segoe WP Semilight by font resolver.
            var wpLightBoldFont = new XFont("Segoe WP Light", fontSize, XFontStyle.Bold);

            // Request of bold font face is mapped to Segoe WP Semilight by font resolver
            // and italic style is simulated.
            var wpLightBoldItalicFont = new XFont("Segoe WP Light", fontSize, XFontStyle.BoldItalic);

            // Bold simulation.
            var wpSemiLightBoldFont = new XFont("Segoe WP Semilight", fontSize, XFontStyle.Bold);

            // Another italic simulation.
            var wpBlackItalicFont = new XFont("Segoe WP Black", fontSize, XFontStyle.Italic);

            // Draw the text.
            const string text = "Sphinx ";
            const double x = 40;
            double y = 50;
            const double dy = 35;

            gfx.DrawString(text + "(Segoe WP Light - regular)", wpLightFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Semilight - regular)", wpSemilightFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP - regular)", wpRegularFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Semibold - regular)", wpSemiboldFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP - bold)", wpBoldFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Black - regular)", wpBlackFont, XBrushes.Black, x, y);
            y += 2 * dy;

            gfx.DrawString(text + "(Segoe WP Light - with italic simulated)", wpLightItalicFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Light - bold)", wpLightBoldFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Light - bold with italic simulated)", wpLightBoldItalicFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Semilight - with bold simulated)", wpSemiLightBoldFont, XBrushes.Black, x, y);
            y += dy;

            gfx.DrawString(text + "(Segoe WP Black - with italic simulated)", wpBlackItalicFont, XBrushes.Black, x, y);
            y += dy;

            // Save the document...
            const string filename = "FontResolver_tempfile.pdf";
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}
