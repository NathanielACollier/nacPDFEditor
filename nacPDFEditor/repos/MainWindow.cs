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

        model.PropertyChanged += (_s, _args) =>
        {
            if (string.Equals(_args.PropertyName, nameof(model.PDFFilePath)))
            {
                OnPDFFilePathChange();
            }
        };

        form.HorizontalGroup(hg =>
        {
            hg.Text("PDF File: ", style: new Style {width = 50})
                .FilePathFor(nameof(model.PDFFilePath));
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
                        .TextFor(nameof(model.PageCountDisplayText), style: new Style {width = 50})
                        .Button("Prev", onClick: onclick_prevPDFPageButton, style: new Style {width = 50})
                        .Button("Next", onClick: onclick_nextPDFPageButton, style: new Style {width = 50})
                        .Button("Rotate Left", onclick_rotateCurrentPageLeft, style: new Style {width = 50});
                }, style: new Style{height = 30})
            .Image(nameof(model.CurrentPageImage));
        }, style: new Style{isVisibleModelName = nameof(model.IsPDFReady)})
        .Display();
    }

    private static async Task onclick_rotateCurrentPageLeft()
    {
        try
        {
            await Task.Run(async () =>
            {
                repos.itextPDFManipulation.rotatePageLeft90(model.PDFFilePath, model.currentPageNumber);

                await reloadPDFFileThenRefreshCurrentPageImageDisplay();

            });
        }
        catch (Exception ex)
        {
            await repos.ErrorHandlerWindow.display(__form, ex, "Rotate Page Left");
        }
    }

    private static async Task onclick_nextPDFPageButton()
    {
        try
        {
            if (model.currentPageNumber > pdfImageReader.PageCount - 2)
            {
                return;
            }

            model.currentPageNumber++;
            var pageImage = await Task.Run(() =>
                {
                    return pdfImageReader.getPageAsImage(model.currentPageNumber);
                });
                    
            model.CurrentPageImage = pageImage;

        }
        catch (Exception ex)
        {
            repos.ErrorHandlerWindow.display(__form, ex, "Next Page Button Click")
                .Wait();
        }

    }

    private static async Task onclick_prevPDFPageButton()
    {
        try
        {
            if (model.currentPageNumber < 1)
            {
                return;
            }

            model.currentPageNumber--;
            var pageImage = await Task.Run(() =>
            {
                return pdfImageReader.getPageAsImage(model.currentPageNumber);
            });
            model.CurrentPageImage = pageImage;
        }
        catch (Exception ex)
        {
            repos.ErrorHandlerWindow.display(__form, ex, "Previous Page button click")
                .Wait();
        }
    }


    private static async Task reloadPDFFileThenRefreshCurrentPageImageDisplay()
    {
        var result = await Task.Run(() =>
        {
            var reader = new repos.PDFDocImageReader(pdfFilePath: model.PDFFilePath);
            // show first page
            var img = reader.getPageAsImage(0);

            return (reader: reader,
                img: img);
        });
        
        model.CurrentPageImage = result.img;
        model.PageCountDisplayText = $"of {pdfImageReader.PageCount - 1}";
    }
    
    
    private static async Task OnPDFFilePathChange()
    {
        if (string.IsNullOrWhiteSpace(model.PDFFilePath))
        {
            return;
        }

        try
        {
            model.IsPDFReady = false;

            __form.Title = $"nacPDFEditor (" + System.IO.Path.GetFileName(model.PDFFilePath) + ")";

            await reloadPDFFileThenRefreshCurrentPageImageDisplay();
            
            model.IsPDFReady = true;
        }
        catch (Exception ex)
        {
            repos.ErrorHandlerWindow.display(__form, ex, $"Load new pdf {model.PDFFilePath}")
                .Wait();
        }



    }
    
}