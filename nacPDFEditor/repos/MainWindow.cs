using nac.Forms;
using nac.Forms.model;

namespace nacPDFEditor.repos;

public class MainWindow
{
    private static models.MainWindow model;
    
    public static async Task run(Form form)
    {
        model = new models.MainWindow();
        form.DataContext = model;

        form.HorizontalGroup(hg =>
        {
            hg.Text("PDF File: ", style: new Style {width = 50})
                .FilePathFor(nameof(model.PDFFilePath), onFilePathChanged: OnPDFFilePathChange);
        })
        .Display();
    }

    private static void OnPDFFilePathChange(string newFilePath)
    {
        // pdf changes
    }
    
}