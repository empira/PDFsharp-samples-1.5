using System;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Graphics
{
    /// <summary>
    /// Shows how to draw graphical paths.
    /// </summary>
    public class Paths : Base
    {
        public void DrawPage(PdfPage page)
        {
            var gfx = XGraphics.FromPdfPage(page);

            DrawTitle(page, gfx, "Paths");

            DrawPathOpen(gfx, 1);
            DrawPathClosed(gfx, 2);
            DrawPathAlternateAndWinding(gfx, 3);
            DrawGlyphs(gfx, 5);
            DrawClipPath(gfx, 6);
        }

        /// <summary>
        /// Strokes an open path.
        /// </summary>
        void DrawPathOpen(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "DrawPath (open)");

            var pen = new XPen(XColors.Navy, Math.PI) { DashStyle = XDashStyle.Dash };

            var path = new XGraphicsPath();
            path.AddLine(10, 120, 50, 60);
            path.AddArc(50, 20, 110, 80, 180, 180);
            path.AddLine(160, 60, 220, 100);
            gfx.DrawPath(pen, path);

            EndBox(gfx);
        }

        /// <summary>
        /// Strokes a closed path.
        /// </summary>
        void DrawPathClosed(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "DrawPath (closed)");

            var pen = new XPen(XColors.Navy, Math.PI);
            pen.DashStyle = XDashStyle.Dash;

            var path = new XGraphicsPath();
            path.AddLine(10, 120, 50, 60);
            path.AddArc(50, 20, 110, 80, 180, 180);
            path.AddLine(160, 60, 220, 100);
            path.CloseFigure();
            gfx.DrawPath(pen, path);

            EndBox(gfx);
        }

        /// <summary>
        /// Draws an alternating and a winding path.
        /// </summary>
        void DrawPathAlternateAndWinding(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "DrawPath (alternate / winding)");

            var pen = new XPen(XColors.Navy, 2.5);

            // Alternate fill mode.
            var path = new XGraphicsPath();
            path.FillMode = XFillMode.Alternate;
            path.AddLine(10, 130, 10, 40);
            path.AddBeziers(new[]{new XPoint(10, 40), new XPoint(30, 0), new XPoint(40, 20), new XPoint(60, 40),
                                   new XPoint(80, 60), new XPoint(100, 60), new XPoint(120, 40)});
            path.AddLine(120, 40, 120, 130);
            path.CloseFigure();
            path.AddEllipse(40, 80, 50, 40);
            gfx.DrawPath(pen, XBrushes.DarkOrange, path);

            // Winding fill mode.
            path = new XGraphicsPath();
            path.FillMode = XFillMode.Winding;
            path.AddLine(130, 130, 130, 40);
            path.AddBeziers(new[]{new XPoint(130, 40), new XPoint(150, 0), new XPoint(160, 20), new XPoint(180, 40),
                                   new XPoint(200, 60), new XPoint(220, 60), new XPoint(240, 40)});
            path.AddLine(240, 40, 240, 130);
            path.CloseFigure();
            path.AddEllipse(160, 80, 50, 40);
            gfx.DrawPath(pen, XBrushes.DarkOrange, path);

            EndBox(gfx);
        }

        /// <summary>
        /// Converts text to path.
        /// </summary>
        void DrawGlyphs(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "Draw Glyphs");

#if CORE
            DrawMessage(gfx, "AddString is not implemented in PDFsharp Core.");
#else
            var path = new XGraphicsPath();
            // AddString is not implemented int PDFsharp Core.
            path.AddString("Hello!", new XFontFamily("Times New Roman"), XFontStyle.BoldItalic, 100, new XRect(0, 0, 250, 140), XStringFormats.Center);
            gfx.DrawPath(new XPen(XColors.Purple, 2.3), XBrushes.DarkOrchid, path);
#endif

            EndBox(gfx);
        }

        /// <summary>
        /// Clips through path.
        /// </summary>
        void DrawClipPath(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "Clip through Path");

#if CORE
            DrawMessage(gfx, "AddString is not implemented in PDFsharp Core.");
#else

            var path = new XGraphicsPath();
            path.AddString("Clip!", new XFontFamily("Verdana"), XFontStyle.Bold, 90, new XRect(0, 0, 250, 140), XStringFormats.Center);
            gfx.IntersectClip(path);

            // Draw a beam of dotted lines.
            var pen = XPens.DarkRed.Clone();
            pen.DashStyle = XDashStyle.Dot;
            for (double r = 0; r <= 90; r += 0.5)
                gfx.DrawLine(pen, 0, 0, 250 * Math.Cos(r / 90 * Math.PI), 250 * Math.Sin(r / 90 * Math.PI));
#endif

            EndBox(gfx);
        }
    }
}
