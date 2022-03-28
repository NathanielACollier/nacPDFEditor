using iText.Kernel.Pdf;

namespace nacPDFEditor.repos;

public static class itextPDFManipulation
{
    public static void rotatePageLeft90(string pdfFilePath, int pageNumber)
    {
        using (var pdfReader = new iText.Kernel.Pdf.PdfReader(filename: pdfFilePath))
        {
            var doc = new iText.Kernel.Pdf.PdfDocument(pdfReader, writer: new PdfWriter(filename: pdfFilePath));

            if (pageNumber > doc.GetNumberOfPages())
            {
                throw new Exception($"Page [number={pageNumber}] is more than number of pages in document");
            }

            var page = doc.GetPage(pageNum: pageNumber);

            page.SetRotation(90);
            
            doc.Close();
        }

    }
    
    
    
    
    
    
    
    
}