using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;

namespace WorkOnPdfObjects
{
    /// <summary>
    /// PDF documents are based internally on objects like dictionaries, arrays,
    /// streams etc. This sample shows how to work directly on these underlying
    /// PDF objects. Use this functionality to achieve PDF features that are not
    /// yet implemented in PDFsharp, e.g. adding an 'open action' to a document.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Get a fresh copy of the sample PDF file.
            const string filename = "Portable Document Format.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), filename);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filename), file, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

            // Read the document into memory for modification.
            var document = PdfReader.Open(filename);

            // The current version of PDFsharp doesn't support the concept of
            // 'actions'. Actions will come in a future version, but if you need them
            // now, you can have them 'handmade'.
            //
            // This sample works on PDF objects directly, therefore some knowledge of
            // the structure of PDF is required.
            // If you are not familiar with the portable document format, first read
            // at least chapter 3 in Adobe's PDF Reference 
            // (http://partners.adobe.com/public/developer/pdf/index_reference.html).
            // If you can read German, I recommend chapter 12 of 'Die PostScript & 
            // PDF-Bibel', a much more interesting reading than the bone-dry Adobe
            // books (http://www.pdflib.com/de/produkte/mehr/bibel/index.html).
            //
            // The sample task is to add an 'open action' to the document so that it
            // starts with the content of page 3 magnified just enough to fit the
            // height of the page within the window.

            // First we have to create a new dictionary that defines the action.
            var dict = new PdfDictionary(document);

            // According to the PDF Reference the dictionary requires two elements.
            // A key /S that specifies the 'GoTo' action,
            // and a key /D that describes the destination.

            // Adding a name as value of key /S is easy.
            dict.Elements["/S"] = new PdfName("/GoTo");

            // The destination is described by an array.
            var array = new PdfArray(document);

            // Set the array as the value of key /D.
            // This makes the array a direct object of the dictionary.
            dict.Elements["/D"] = array;

            // Now add the elements to the array. According to the PDF Reference
            // there must be three for a page as the target of a 'GoTo' action.
            // The first element is an indirect reference to the destination page.
            // To add an indirect reference to the page three, we first need the 
            // PdfReference object of that page.
            // (The index in the Pages collection is zero based, therefore Pages[2])
            var iref = PdfInternals.GetReference(document.Pages[2]);

            // Add the reference to the third page as the first array element.
            // Adding the iref (instead of the PdfPage object itself) makes it an 
            // indirect reference.
            array.Elements.Add(iref);

            // The second element is the name /FitV to indicate 'fit vertically'. 
            array.Elements.Add(new PdfName("/FitV"));

            // /FitV requires the horizontal coordinate that will be positioned at the
            // left edge of the window. We set -32768 because Acrobat uses this value
            // to show the full page (it means 'left aligned' anyway if the window is
            // so small that a horizontal scroll bar is required).
            array.Elements.Add(new PdfInteger(-32768));

            // Now that the action dictionary is complete, we can add it to the
            // document's object table.
            // Adding an object to the object table makes it an indirect object.
            document.Internals.AddObject(dict);

            // Finally we must add the action dictionary to the /OpenAction key of
            // the document's catalog as an indirect value.
            document.Internals.Catalog.Elements["/OpenAction"] =
              PdfInternals.GetReference(dict);

            // Using PDFsharp we never deal with object numbers. We simply put the
            // objects together and the PDFsharp framework does the rest.

            // Save the document...
            document.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}