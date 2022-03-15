using nac.Forms;
using nac.Forms.model;

namespace nacPDFEditor.repos;

public static class MainWindow
{
    private static nac.Forms.Form __form; // mainly use this to set Title
    private static models.MainWindow model;
    private static repos.PDFDocImageReader pdfImageReader;
    
    public static async Task run(Form form)
    {
        model = new models.MainWindow();
        model.IsPDFReady = false;
        form.DataContext = model;
        __form = form;

        form.HorizontalGroup(hg =>
        {
            hg.Text("PDF File: ", style: new Style {width = 50})
                .FilePathFor(nameof(model.PDFFilePath), onFilePathChanged: OnPDFFilePathChange);
        })
        .VerticalGroup(dependOnPDFVG =>
        {
            dependOnPDFVG.HorizontalGroup(hg =>
                {
                    hg.Text("Page:", new Style {width = 50})
                        .TextBoxFor(nameof(model.currentPageNumber), convertFromUIToModel: (str) =>
                        {
                            if (int.TryParse(str, out int myNum))
                            {
                                return myNum;
                            }

                            return 0;
                        }, style: new Style {width = 50})
                        .TextFor(nameof(model.PageCountDisplayText), style:new Style{width = 50})
                        .Button("Prev", onClick: onclick_prevPDFPageButton, style:new Style{width = 50})
                        .Button("Next", onClick: onclick_nextPDFPageButton, style:new Style{width = 50});
                }, style: new Style{height = 30})
            .Image(nameof(model.CurrentPageImage));
        }, style: new Style{isVisibleModelName = nameof(model.IsPDFReady)})
        .Display();
    }

    private static void onclick_nextPDFPageButton(object obj)
    {
        if (model.currentPageNumber > pdfImageReader.PageCount - 2)
        {
            return;
        }

        model.currentPageNumber++;
        model.CurrentPageImage = pdfImageReader.getPageAsImage(model.currentPageNumber);
    }

    private static void onclick_prevPDFPageButton(object obj)
    {
        if (model.currentPageNumber < 1)
        {
            return;
        }

        model.currentPageNumber--;
        model.CurrentPageImage = pdfImageReader.getPageAsImage(model.currentPageNumber);
    }

    private static void OnPDFFilePathChange(string newFilePath)
    {
        if (string.IsNullOrWhiteSpace(newFilePath))
        {
            return;
        }
        model.IsPDFReady = false;

        __form.Title = $"nacPDFEditor (" + System.IO.Path.GetFileName(newFilePath) + ")";

        pdfImageReader = new repos.PDFDocImageReader(pdfFilePath: newFilePath);
        // show first page
        model.CurrentPageImage = pdfImageReader.getPageAsImage(0);
        model.PageCountDisplayText = $"of {pdfImageReader.PageCount-1}";

        model.IsPDFReady = true;
    }
    
}