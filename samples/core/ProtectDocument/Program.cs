using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf.IO;

namespace ProtectDocument
{
    /// <summary>
    /// This sample shows how to protect a document with a password.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Get a fresh copy of the sample PDF file.
            const string filenameSource = "HelloWorld.pdf";
            const string filenameDest = "HelloWorld_tempfile.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), filenameDest);
            File.Copy(Path.Combine("../../../../assets/PDFs/", filenameSource), file, true);

            // Remove ReadOnly attribute from the copy.
            File.SetAttributes(file, File.GetAttributes(file) & ~FileAttributes.ReadOnly);

            // Open an existing document. Providing an unrequired password is ignored.
            var document = PdfReader.Open(filenameDest, "some text");

            var securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = "user";
            securitySettings.OwnerPassword = "owner";

            // Don't use 40 bit encryption unless needed for compatibility.
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            // Save the document...
            document.Save(filenameDest);
            // ...and start a viewer.
            Process.Start(filenameDest);
        }
    }
}