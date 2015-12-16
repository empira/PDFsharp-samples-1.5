Sample Project for Issue Submissions
====================================

Preface
-------
It is very difficult for us to fix an issue which we cannot replicate.
We cannot even test whether the issue was resolved if we cannot replicate it.
In the best case, when submitting an issue report provide us a solution that
allows us to replicate the issue.

Many people showed us a few lines of their code on the forum.
In many cases, the code was running fine after we built an application around
the code snippet.
The problem was with the code that was not shown - and we wasted our time.


Make your issue replicable
--------------------------
Send us a solution that contains as little code as possible and that allows
us to replicate the issue.
With as few steps as possible.
Include a description that allows us to replicate the issue.


To get you started
------------------
This solution basically contains the PDFsharp Hello World sample.
Change as you wish to make the sample replicate the issue you wish us to fix.
Remove what you do not need.


How it works
------------
There are three projects: for the CORE, GDI, and WPF build respectively.
The three projects share the same source file Program.cs.
Just add your code to this source file.

If your code uses platform-specific routines, feel free to wrap it
with "#if GDI" or "#if WPF".
This will ensure that all three projects compile and we can see which build
we have to use.


Notes
-----
Sometimes issues are platform-specific, due to different implementations
in PDFsharp.
Sometimes the issue is with the platform-specific routines that PDFsharp calls.
The Issue Submission Template allows you to test your code with three builds
of PDFsharp.
If the issue does not apply to all platforms, maybe you can work around the
problem by switching to a different build.


Information about NuGet Packages
================================
The PDFsharp and MigraDoc samples refer to NuGet packages.

In the best case, you already have installed the NuGet Package Manager (NPM)
and you also have set the NPM to download missing packages during compile.

In the worst case you didn’t even install the NuGet Package Manager yet.
For further information, please refer to this page:
http://pdfsharp.net/wiki/NuGetPackages.ashx

