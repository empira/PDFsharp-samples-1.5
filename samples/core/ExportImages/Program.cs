using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using PdfSharp.Pdf.IO;
#if !WPF
using System.Windows.Forms;
#else
using System.Windows;
#endif

namespace ExportImages
{
    /// <summary>
    /// This sample shows how to export JPEG images from a PDF file.
    /// </summary>
    class Program
    {
        static void Main()
        {
            const string filename = "../../../../assets/PDFs/SomeLayout.pdf";

            var document = PdfReader.Open(filename);

            var imageCount = 0;
            // Iterate the pages.
            foreach (var page in document.Pages)
            {
                // Get the resources dictionary.
                var resources = page.Elements.GetDictionary("/Resources");
                if (resources == null) 
                    continue;

                // Get the external objects dictionary.
                var xObjects = resources.Elements.GetDictionary("/XObject");
                if (xObjects == null)
                    continue;

                var items = xObjects.Elements.Values;
                // Iterate the references to external objects.
                foreach (var item in items)
                {
                    var reference = item as PdfReference;
                    if (reference == null)
                        continue;

                    var xObject = reference.Value as PdfDictionary;
                    // Is external object an image?
                    if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                    {
                        ExportImage(xObject, ref imageCount);
                    }
                }
            }

            MessageBox.Show(imageCount + " images exported.", "Export Images");
        }

        /// <summary>
        /// Currently extracts only JPEG images.
        /// </summary>
        static void ExportImage(PdfDictionary image, ref int count)
        {
            var filter = image.Elements.GetValue("/Filter");
            // Do we have a filter array?
            var array = filter as PdfArray;
            if (array != null)
            {
                // PDF files sometimes contain "zipped" JPEG images.
                if (array.Elements.GetName(0) == "/FlateDecode" &&
                    array.Elements.GetName(1) == "/DCTDecode")
                {
                    ExportJpegImage(image, true, ref count);
                    return;
                }

                // TODO Deal with other encodings like "/FlateDecode" + "/CCITTFaxDecode"
            }

            // Do we have a single filter?
            var name = filter as PdfName;
            if (name != null)
            {
                var decoder = name.Value;
                switch (decoder)
                {
                    case "/DCTDecode":
                        ExportJpegImage(image, false, ref count);
                        break;

                    case "/FlateDecode":
                        ExportAsPngImage(image, ref count);
                        break;

                    // TODO Deal with other encodings like "/CCITTFaxDecode"
                }
            }
        }

        /// <summary>
        /// Exports a JPEG image.
        /// </summary>
        static void ExportJpegImage(PdfDictionary image, bool flateDecode, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            var stream = flateDecode ? Filtering.Decode(image.Stream.Value, "/FlateDecode") : image.Stream.Value;
            var fs = new FileStream(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();
        }

        /// <summary>
        /// Exports image in PNG format.
        /// </summary>
        static void ExportAsPngImage(PdfDictionary image, ref int count)
        {
            var width = image.Elements.GetInteger(PdfImage.Keys.Width);
            var height = image.Elements.GetInteger(PdfImage.Keys.Height);
            var bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);

            // TODO: You can put the code here that converts from PDF internal image format to a Windows bitmap.
            // and use GDI+ to save it in PNG format.
            // It is the work of a day or two for the most important formats. Take a look at the file
            // PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
            // We don't need that feature at the moment and therefore will not implement it.
            // If you write the code for exporting images I would be pleased to publish it in a future release
            // of PDFsharp.
        }
    }
}