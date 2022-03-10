# NAC PDF Editor

## Plan
+ Developed on linux, but run on Windows/MacOS
+ These are the planned features
    + PDF Merge using itextsharp
        + [] Merge before/after a specific page
        + [] Merge at beginning or end
    + PDF Page Rotation
        + [] Rotate left (90)
        + [] Rotate Right (90)
        + [] Flip (Rotate left twice 180)
    + PDF Page Move
        + [] Move a page before or after another page
    + PDF Page Deletion
        + [] Delete a page number

## Dependencies
+ Avalonia  (via nac.forms)
+ ImageSharp (converting raw pixels to bitmap for Avalonia Image control)
+ docnet.core (Render PDF pages as Images)
    + Is a wrapper for Google PDFIum
+ itextsharp (PDF Manipulation)
    