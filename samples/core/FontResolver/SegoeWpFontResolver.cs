using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PdfSharp.Fonts;
using WPFonts;

namespace FontResolver
{
    /// <summary>
    /// Maps font requests for a SegoeWP font to a bunch of 6 specific font files.
    /// These 6 fonts are embedded as resources in the WPFonts assembly.
    /// </summary>
    public class SegoeWpFontResolver : IFontResolver
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// The font family names that can be used in the constructor of XFont.
        /// Used in the first parameter of ResolveTypeface.
        /// Family names are given in lower case because the implementation of SegoeWpFontResolver ignores case.
        /// </summary>
        static class FamilyNames
        {
            public const string SegoeWPLight = "segoe wp light";
            public const string SegoeWPSemilight = "segoe wp semilight";
            public const string SegoeWP = "segoe wp";
            public const string SegoeWPSemibold = "segoe wp semibold";
            public const string SegoeWPBold = "segoe wp bold";
            public const string SegoeWPBlack = "segoe wp black";
        }

        /// <summary>
        /// The internal names that uniquely identify a font's type faces (i.e. a physical font file).
        /// Used in the first parameter of the FontResolverInfo constructor.
        /// </summary>
        static class FaceNames
        {
            /// Used in the first parameter of the FontResolverInfo constructor.
            public const string SegoeWPLight = "SegoeWPLight";
            public const string SegoeWPSemilight = "SegoeWPSemilight";
            public const string SegoeWP = "SegoeWP";
            public const string SegoeWPSemibold = "SegoeWPSemibold";
            public const string SegoeWPBold = "SegoeWPBold";
            public const string SegoeWPBlack = "SegoeWPBlack";
        }

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Converts specified information about a required typeface into a specific font.
        /// </summary>
        /// <param name="familyName">Name of the font family.</param>
        /// <param name="isBold">Set to <c>true</c> when a bold fontface is required.</param>
        /// <param name="isItalic">Set to <c>true</c> when an italic fontface is required.</param>
        /// <returns>
        /// Information about the physical font, or null if the request cannot be satisfied.
        /// </returns>
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Note: PDFsharp calls ResolveTypeface only once for each unique combination
            // of familyName, isBold, and isItalic.

            // In this sample we use 6 fonts from the Segoe font family which come with the
            // Windows Phone SDK. These fonts are pretty and well designed and their
            // font files are much smaller than their Windows counterparts.

            // What this implementation do:
            // * if both isBold and isItalic is false the regular font is resolved
            // * if isBold is true either a bolder font is resolved or the bold request is ignored
            // * if isItalic is true italic simulation is turned on because there is no italic font
            //
            // Currently there are two minor design flaws/bugs in PDFshrap that will be fixed later:
            // If the same font is used with and without italic simulation two subsets of it 
            // are embedded in the PDF file (instead of one subset with the glyphs of both usages).
            // If an XFont is italic and the resolved font is not an italic font, italic simulation is
            // always used (i.e. you cannot turn italic simulation off in ResolveTypeface).
            // One more thing: TrueType font collections are also not yet supported.

            string lowerFamilyName = familyName.ToLowerInvariant();
            // Looking for a Segoe WP font?
            if (lowerFamilyName.StartsWith("segoe wp"))
            {
                // Bold simulation is not recommended, but used here for demonstration.
                 bool simulateBold = false;

                // Since Segoe WP typefaces do not contain any italic font
                // always simulate italic if it is requested.
                bool simulateItalic = isItalic;

                string faceName;

                // In this sample family names are case sensitive. You can relax this in your own implementation
                // and make them case insensitive.
                switch (lowerFamilyName)
                {
                    case FamilyNames.SegoeWPLight:
                        // Just for demonstration use 'Semilight' if bold is requested.
                        if (isBold)
                            goto case FamilyNames.SegoeWPSemilight;
                        faceName = FaceNames.SegoeWPLight;
                        break;

                    case FamilyNames.SegoeWPSemilight:
                        // Demonstrate bold simulation.
                        if (isBold)
                            simulateBold = true;
                        faceName = FaceNames.SegoeWPSemilight;
                        break;

                    case FamilyNames.SegoeWP:
                        // Use font 'Bold' if bold is requested.
                        if (isBold)
                            goto UseSegoeWPBold;
                        faceName = FaceNames.SegoeWP;
                        break;

                    case FamilyNames.SegoeWPSemibold:
                        // Do not care about bold for semibold.
                        faceName = FaceNames.SegoeWPSemibold;
                        break;

                    case FamilyNames.SegoeWPBold:
                        // Just for demonstration use font 'Black' if bold is requested.
                        if (isBold)
                            goto case FamilyNames.SegoeWPBlack;
                    UseSegoeWPBold:
                        faceName = FaceNames.SegoeWPBold;
                        break;

                    case FamilyNames.SegoeWPBlack:
                        // Do not care about bold for black.
                        faceName = FaceNames.SegoeWPBlack;
                        break;

                    default:
                        Debug.Assert(false, "Unknown Segoe WP font: " + lowerFamilyName);
                        goto case FamilyNames.SegoeWP;  // Alternatively throw an exception in this case.
                }

                // Tell the caller the effective typeface name and whether bold  or italic should be simulated.
                return new FontResolverInfo(faceName, simulateBold, simulateItalic);
            }

            // Return null means that the typeface cannot be resolved and PDFsharp stops working.
            // Alternatively forward call to PlatformFontResolver.
            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }

        /// <summary>
        /// Gets the bytes of a physical font with specified typeface name.
        /// </summary>
        /// <param name="faceName">A face name previously retrieved by ResolveTypeface.</param>
        /// <returns>
        /// The bytes of the font.
        /// </returns>
        public byte[] GetFont(string faceName)
        {
            // Note: PDFsharp never calls GetFont twice with the same face name.

            // Return the bytes of a font.
            switch (faceName)
            {
                case FaceNames.SegoeWPLight:
                    return FontDataHelper.SegoeWPLight;

                case FaceNames.SegoeWPSemilight:
                    return FontDataHelper.SegoeWPSemilight;

                case FaceNames.SegoeWP:
                    return FontDataHelper.SegoeWP;

                case FaceNames.SegoeWPSemibold:
                    return FontDataHelper.SegoeWPSemibold;

                case FaceNames.SegoeWPBold:
                    return FontDataHelper.SegoeWPBold;

                case FaceNames.SegoeWPBlack:
                    return FontDataHelper.SegoeWPBlack;
            }
            // PDFsharp never calls GetFont with a face name that was not returned by ResolveTypeface.
            throw new ArgumentException(String.Format("Invalid typeface name '{0}'", faceName));
        }
    }
}
