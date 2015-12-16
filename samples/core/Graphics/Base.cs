using System;
using System.Globalization;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Graphics
{
    /// <summary>
    /// The base class with some helper functions.
    /// </summary>
    public class Base
    {
        protected XColor BackColor;
        protected XColor BackColor2;
        protected XColor ShadowColor;
        protected double BorderWidth;
        protected XPen BorderPen;

        protected Base()
        {
            BackColor = XColors.Ivory;
            BackColor2 = XColors.WhiteSmoke;

            BackColor = XColor.FromArgb(212, 224, 240);
            BackColor2 = XColor.FromArgb(253, 254, 254);

            ShadowColor = XColors.Gainsboro;
            BorderWidth = 4.5;
            BorderPen = new XPen(XColor.FromArgb(94, 118, 151), BorderWidth);
        }

        /// <summary>
        /// Draws the page title and footer.
        /// </summary>
        public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        {
            var rect = new XRect(new XPoint(), gfx.PageSize);
            rect.Inflate(-10, -15);
            var font = new XFont("Verdana", 14, XFontStyle.Bold);
            gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

            rect.Offset(0, 5);
            font = new XFont("Verdana", 8, XFontStyle.Italic);
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Far;
            gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);

            font = new XFont("Verdana", 8);
            format.Alignment = XStringAlignment.Center;
            gfx.DrawString(Program.Document.PageCount.ToString(CultureInfo.InvariantCulture), font, XBrushes.DarkOrchid, rect, format);

            Program.Document.Outlines.Add(title, page, true);
        }

        /// <summary>
        /// Draws a sample box.
        /// </summary>
        public void BeginBox(XGraphics gfx, int number, string title)
        {
            const int dEllipse = 15;
            var rect = new XRect(0, 20, 300, 200);
            if (number % 2 == 0)
                rect.X = 300 - 5;
            rect.Y = 40 + ((number - 1) / 2) * (200 - 5);
            rect.Inflate(-10, -10);
            var rect2 = rect;
            rect2.Offset(BorderWidth, BorderWidth);
            gfx.DrawRoundedRectangle(new XSolidBrush(ShadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
            var brush = new XLinearGradientBrush(rect, BackColor, BackColor2, XLinearGradientMode.Vertical);
            gfx.DrawRoundedRectangle(BorderPen, brush, rect, new XSize(dEllipse, dEllipse));
            rect.Inflate(-5, -5);

            var font = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);

            rect.Inflate(-10, -5);
            rect.Y += 20;
            rect.Height -= 20;

            _state = gfx.Save();
            gfx.TranslateTransform(rect.X, rect.Y);
        }

        public void EndBox(XGraphics gfx)
        {
            gfx.Restore(_state);
        }

        /// <summary>
        /// Gets a five-pointed star with the specified size and center.
        /// </summary>
        protected static XPoint[] GetPentagram(double size, XPoint center)
        {
            var points = (XPoint[])Pentagram.Clone();
            for (var idx = 0; idx < 5; idx++)
            {
                points[idx].X = points[idx].X * size + center.X;
                points[idx].Y = points[idx].Y * size + center.Y;
            }
            return points;
        }

        /// <summary>
        /// Gets a normalized five-pointed star.
        /// </summary>
        static XPoint[] Pentagram
        {
            get
            {
                if (_pentagram == null)
                {
                    var order = new[] { 0, 3, 1, 4, 2 };
                    _pentagram = new XPoint[5];
                    for (var idx = 0; idx < 5; idx++)
                    {
                        var rad = order[idx] * 2 * Math.PI / 5 - Math.PI / 10;
                        _pentagram[idx].X = Math.Cos(rad);
                        _pentagram[idx].Y = Math.Sin(rad);
                    }
                }
                return _pentagram;
            }
        }
        static XPoint[] _pentagram;

        protected void DrawMessage(XGraphics gfx, string message)
        {
            var font = new XFont("PlatformDefault", 12, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));

            gfx.DrawString(message, font, XBrushes.DarkSlateGray, 10, 10);
        }

        XGraphicsState _state;
    }
}
