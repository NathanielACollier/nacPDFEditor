namespace nacPDFEditor.models;

public class MainWindow : nac.Forms.model.ViewModelBase
{

    public string PDFFilePath
    {
        get { return GetValue(() => PDFFilePath); }
        set { SetValue(() => PDFFilePath, value);}
    }

    public bool IsPDFReady
    {
        get { return GetValue(() => IsPDFReady); }
        set { SetValue(() => IsPDFReady, value);}
    }


    public int currentPageNumber
    {
        get { return GetValue(() => currentPageNumber); }
        set { SetValue(() => currentPageNumber, value);}
    }

    public byte[] CurrentPageImage
    {
        get { return GetValue(() => CurrentPageImage); }
        set { SetValue(() => CurrentPageImage, value);}
    }

    public string PageCountDisplayText
    {
        get { return GetValue(() => PageCountDisplayText); }
        set { SetValue(() => PageCountDisplayText, value);}
    }
}