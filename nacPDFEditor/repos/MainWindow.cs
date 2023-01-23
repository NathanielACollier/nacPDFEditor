using iText.Kernel.Pdf;
using nac.Forms;
using nac.Forms.model;

namespace nacPDFEditor.repos;

public static class MainWindow
{
    private static nac.Forms.Form __form; // mainly use this to set Title
    private static models.MainWindow model;
    private static repos.PDFDocImageReader pdfImageReader;
    private static byte[] currentPDFData;
    
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
                        }, style: new Style {width = 50},
                        onKeyPress: async (_keyArgs)=>
                        {
                            try
                            {
                                if (_keyArgs.Key == Avalonia.Input.Key.Enter)
                                {
                                    await refreshCurrentPage(); // page number changes by them entering in textbox, so just refresh
                                }
                            }catch(Exception ex)
                            {
                                repos.ErrorHandlerWindow.display(__form, ex, "Page Number Textbox Enter Press");
                            }

                        })
                        .TextFor(nameof(model.PageCountDisplayText), style: new Style {width = 50})
                        .Button(_f=> _f.Image(nameof(model.ImagesLeftArrow)), onClick: onclick_prevPDFPageButton,
                                    style: new Style {width = 50,
                                    TooltipText="Previous Page"})
                        .Button(_f=> _f.Image(nameof(model.ImagesRightArrow)), onClick: onclick_nextPDFPageButton,
                                    style: new Style {width = 50,
                                    TooltipText="Next Page"})
                        .Button(_f=> _f.Image(nameof(model.ImagesRotateLeft)), onclick_rotateCurrentPageLeft,
                                    style: new Style {width = 50,
                                    TooltipText="Rotate Current Page Left"});
                }, style: new Style{height = 30})
            .Image(nameof(model.CurrentPageImage));
        }, style: new Style{isVisibleModelName = nameof(model.IsPDFReady)})
        .Display();
    }

    private static async Task onclick_rotateCurrentPageLeft()
    {
        try
        {
            var pdfData = await Task.Run(async () =>
            {
                using (var ms = new System.IO.MemoryStream(currentPDFData))
                {
                    // itext thinks of pages as 1 thorugh end
                    // where some of the other stuff thinks of it as starting with 0
                    var alteredPDFData = repos.itextPDFManipulation.rotatePageLeft90(pdfStream: ms, model.currentPageNumber + 1);

                    return alteredPDFData;
                }

            });
            
            await displayPdfDataAtPage(pdfData, model.currentPageNumber);
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
            await refreshCurrentPage();

        }
        catch (Exception ex)
        {
            await repos.ErrorHandlerWindow.display(__form, ex, "Next Page Button Click");
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
            await refreshCurrentPage();
        }
        catch (Exception ex)
        {
            await repos.ErrorHandlerWindow.display(__form, ex, "Previous Page button click");
        }
    }


    private static async Task refreshCurrentPage()
    {
        var pageImage = await Task.Run(() =>
        {
            return pdfImageReader.getPageAsImage(model.currentPageNumber);
        });
        model.CurrentPageImage = pageImage;
    }

    private static async Task reloadPDFFileThenRefreshCurrentPageImageDisplay()
    {
        var result = await Task.Run(() =>
        {
            // read in the PDF bytes
            var pdfFileData = System.IO.File.ReadAllBytes(model.PDFFilePath);
            return pdfFileData;
        });

        await displayPdfDataAtPage(result, 0);
    }

    private static async Task displayPdfDataAtPage(byte[] pdfData, int pageNumber)
    {
        var result = await Task.Run(() =>
        {
            var reader = new repos.PDFDocImageReader(pdfData: pdfData);

            var img = reader.getPageAsImage(pageNumber);

            return (reader: reader,
                img: img);
        });
        
        model.CurrentPageImage = result.img;
        pdfImageReader = result.reader;
        currentPDFData = pdfData; // save this for manipulation
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
            await repos.ErrorHandlerWindow.display(__form, ex, $"Load new pdf {model.PDFFilePath}");
        }



    }
    
}