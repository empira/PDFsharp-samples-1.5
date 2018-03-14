Welcome to PDFsharp and MigraDoc Foundation
===========================================

PDFsharp & MigraDoc Foundation Support Forum
--------------------------------------------
The right place to search for answers and to ask new questions:
http://forum.pdfsharp.net/


The PDFsharp & MigraDoc Web Site
--------------------------------
Here's the homepage:
http://www.pdfsharp.net/

Here's the PDFsharp & MigraDoc Wiki:
http://www.pdfsharp.net/wiki/

Please note: the Wiki introduces many of the PDFsharp and MigraDoc samples
with screen shots, code snippets, generated PDF files, etc.


Information about the PDFsharp samples
======================================
Using the samples with NuGet Packages
-------------------------------------
The samples can be used with the NuGet packages. This is the simple way.
- Double-click the SLN file to open it with Visual Studio.
- Verify under "Tools" => "Extensions and Updates..." that you have the
  latest version of the NuGet Package Manager installed.
- In the Solution Explorer, right-click the Solution and select
  "Manage NuGet Packages for Solution..."
- You should see a yellow bar with a "Restore" button right under the caption
  of the "Manage NuGet Packages" dialog. Click "Restore" to get the packages.
- Click "Close" to leave the "Manage NuGet Packages" dialog. The samples
  should now compile and run without errors.
- If there are problems with HelloWorld.pdf: Unzip the file from samples\assets\pdfs\HelloWorld.zip.

Using the samples with the PDFsharp source package
--------------------------------------------------
If you also downloaded the complete source for PDFsharp, then you can use the
source code. Right-click the solution and select "Add" =>
"Existing Project..." and select the projects.
In the Solution Explorer open the sample you want to run and open its
References folder. Remove the broken references to the NuGet packages.
Then right-click "References" and select "Add Reference...", then in the
"Reference Manager" click "Solution" and check the projects you need,
e.g. "PdfSharp" and "PdfSharp.Charting" or "PdfSharp-gdi" and
"PdfSharp.Charting-gdi".
Adding references to the projects from the source package will allow you
to step through the PDFsharp source code and eventually make changes.
