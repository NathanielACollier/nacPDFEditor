namespace nacPDFEditor.models;

public class MainWindow : nac.Forms.model.ViewModelBase
{

    public string PDFFilePath
    {
        get { return GetValue(() => PDFFilePath); }
        set { SetValue(() => PDFFilePath, value);}
    }
    
    
    
}