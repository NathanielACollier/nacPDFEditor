using iText.Kernel.Pdf;

namespace nacPDFEditor.repos;

public static class itextPDFManipulation
{

    private delegate void manipulatePDFDelegate(iText.Kernel.Pdf.PdfReader pdfReader,
                        System.IO.MemoryStream outStream,
                        iText.Kernel.Pdf.PdfDocument doc);

    private static byte[] manipulatePDFDocument(System.IO.Stream pdfStream, manipulatePDFDelegate actionToTake)
    {
        using (var pdfReader = new iText.Kernel.Pdf.PdfReader(pdfStream))
        {
            using (var outStr = new System.IO.MemoryStream())
            {
                var doc = new iText.Kernel.Pdf.PdfDocument(pdfReader, writer: new PdfWriter(outStr));

                actionToTake(pdfReader: pdfReader,
                    outStream: outStr,
                    doc: doc);

                doc.Close();
                return outStr.ToArray();
            }

        }
    }


    public static byte[] rotatePageLeft90(System.IO.Stream pdfStream, int pageNumber)
    {
        return manipulatePDFDocument(pdfStream, (reader, outStr, doc) =>
        {
            if (pageNumber > doc.GetNumberOfPages())
            {
                throw new Exception($"Page [number={pageNumber}] is more than number of pages in document");
            }

            var page = doc.GetPage(pageNum: pageNumber);

            rotatePageDegreesTowardLeft(page, 90);
        });
    }



    private static void rotatePageDegreesTowardLeft(PdfPage page, int degrees)
    {
        int currentRotation = page.GetRotation();

        int newRotation = currentRotation + degrees;

        if(newRotation > 360)
        {
            newRotation = 360 - newRotation;
        }

        page.SetRotation(newRotation);
    }
}