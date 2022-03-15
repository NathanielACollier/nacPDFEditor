﻿using nac.Forms;
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
                hg.Text("Page:")
                    .TextBoxFor(nameof(model.currentPageNumber), convertFromUIToModel: (str) =>
                    {
                        if (int.TryParse(str, out int myNum))
                        {
                            return myNum;
                        }

                        return 0;
                    })
                    .Text($"of {pdfImageReader.PageCount}");
            })
            .Image(nameof(model.CurrentPageImage));
        }, style: new Style{isVisibleModelName = nameof(model.IsPDFReady)})
        .Display();
    }

    private static void OnPDFFilePathChange(string newFilePath)
    {
        model.IsPDFReady = false;

        __form.Title = $"nacPDFEditor (" + System.IO.Path.GetFileName(newFilePath) + ")";

        pdfImageReader = new repos.PDFDocImageReader(pdfFilePath: newFilePath);
        // show first page
        model.CurrentPageImage = pdfImageReader.getPageAsImage(0);
    }
    
}