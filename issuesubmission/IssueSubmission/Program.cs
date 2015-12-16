using System;
using System.Collections.Generic;
using System.Diagnostics;
#if GDI
using System.Drawing;
#endif
#if WPF
using System.Windows.Media.Imaging;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace IssueSubmission
{
    class Program
    {
        static void Main()
        {
            // Create a new PDF document 
            PdfDocument document = new PdfDocument();

            // Create an empty page 
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing 
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font 
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
            // Draw the text 
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
                           new XRect(0, 0, page.Width, page.Height),
                           XStringFormats.Center);

#if GDI
            // Using GDI-specific routines.
            // Make sure to use "#if GDI" for any usings you add for platform-specific code.
            {
                // Just for demo purposes, we create an image and draw it.
                Image image = new Bitmap(@"..\..\Sample.png");
                // XImage.FromGdiPlusImage is available with the GDI build only.
                XImage gdiImage = XImage.FromGdiPlusImage(image);
                gdiImage.Interpolate = false;
                gfx.DrawImage(gdiImage, 0, 0, 16, 16);
            }
#endif

#if WPF
            // Using WPF-specific routines.
            // Make sure to use "#if GDI" for any usings you add for platform-specific code.
            {
                // Just for demo purposes, we create an image and draw it.
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(@"..\..\Sample.png", UriKind.Relative);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                // XImage.FromBitmapSource is available with the WPF build only.
                XImage gdiImage = XImage.FromBitmapSource(image);
                gdiImage.Interpolate = false;
                gfx.DrawImage(gdiImage, 0, 0, 16, 16);
            }
#endif

            // Save the document... 
            const string filename = "HelloWorld.pdf";
            document.Save(filename);

            // ...and start a viewer. 
            Process.Start(filename);
        }
    }
}
