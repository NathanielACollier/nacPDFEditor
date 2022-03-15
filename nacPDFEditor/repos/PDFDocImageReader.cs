using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace nacPDFEditor.repos;

public class PDFDocImageReader
{
    /*
     see example here: https://stackoverflow.com/questions/12831742/convert-pdf-to-image-without-using-ghostscript-dll
     library is at: https://github.com/GowenGit/docnet
     Examples are here: https://github.com/GowenGit/docnet/blob/master/examples/nuget-usage/NugetUsageAnyCpu/PdfToImageExamples.cs
     */
    private Docnet.Core.Readers.IDocReader docReader;
    public int PageCount { get; }

    public PDFDocImageReader(string pdfFilePath)
    {
        // load up a pdf from a filepath
        this.docReader = Docnet.Core.DocLib.Instance.GetDocReader(pdfFilePath, new PageDimensions(1080, 1920));

        this.PageCount = this.docReader.GetPageCount();
    }

    public byte[] getPageAsImage(int pageNumber)
    {
        using (var pageReader = docReader.GetPageReader(pageNumber))
        {
            var rawBytes = pageReader.GetImage(); // formats it as Bgra32
            
            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();
            
            // THis is the best documentation I found on using ImageSharp and pdfium 
            //   see: https://stackoverflow.com/questions/23905169/how-to-convert-pdf-files-to-images

            // use ImageSharp to interpret those raw bytes in B-G-R-A format into a bitmap that avalonia can understand
            var img = SixLabors.ImageSharp.Image.LoadPixelData<Bgra32>(rawBytes, width, height);
            // Set the background to white, otherwise it's black. https://github.com/SixLabors/ImageSharp/issues/355#issuecomment-333133991
            img.Mutate(x => x.BackgroundColor(Color.White));
            
            using (var ms = new System.IO.MemoryStream())
            {
                img.SaveAsBmp(ms);
                var imgData = ms.ToArray();
                return imgData;
            }
        }// end of page reader
    }
}