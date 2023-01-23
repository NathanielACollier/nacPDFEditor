using iText.Kernel.Pdf;

namespace nacPDFEditor.repos;

public static class itextPDFManipulation
{
    public static byte[] rotatePageLeft90(System.IO.Stream pdfStream, int pageNumber)
    {
        using (var pdfReader = new iText.Kernel.Pdf.PdfReader(pdfStream))
        {
            using (var outStr = new System.IO.MemoryStream())
            {
                var doc = new iText.Kernel.Pdf.PdfDocument(pdfReader, writer: new PdfWriter(outStr));

                if (pageNumber > doc.GetNumberOfPages())
                {
                    throw new Exception($"Page [number={pageNumber}] is more than number of pages in document");
                }

                var page = doc.GetPage(pageNum: pageNumber);

                rotatePageDegrees(page, 90);
            
                doc.Close();
                return outStr.ToArray();
            }

        }

    }

    private static void rotatePageDegrees(PdfPage page, int degrees)
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