using System;
using System.IO;
using System.Reflection;

// ReSharper disable InconsistentNaming

namespace WPFonts
{
    /// <summary>
    /// Helper class that returns fonts from embedded resources as byte arrays.
    /// </summary>
    public static class FontDataHelper
    {
        /// <summary>
        /// Gets the Segoe WP Light font.
        /// </summary>
        public static byte[] SegoeWPLight
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP-Light.ttf"); }
        }

        /// <summary>
        /// Gets the Segoe WP Semilight font.
        /// </summary>
        public static byte[] SegoeWPSemilight
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP-Semilight.ttf"); }
        }

        /// <summary>
        /// Gets the Segoe WP font.
        /// </summary>
        public static byte[] SegoeWP
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP.ttf"); }
        }

        /// <summary>
        /// Gets the Segoe WP Semibold font.
        /// </summary>
        public static byte[] SegoeWPSemibold
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP-Semibold.ttf"); }
        }

        /// <summary>
        /// Gets the Segoe WP Bold font.
        /// </summary>
        public static byte[] SegoeWPBold
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP-Bold.ttf"); }
        }

        /// <summary>
        /// Gets the Segoe WP Black font.
        /// </summary>
        public static byte[] SegoeWPBlack
        {
            get { return LoadFontData("WPFonts.Fonts.SegoeWP-Black.ttf"); }
        }

        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        static byte[] LoadFontData(string name)
        {
            Assembly assembly = typeof(FontDataHelper).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                var count = (int)stream.Length;
                var data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}
