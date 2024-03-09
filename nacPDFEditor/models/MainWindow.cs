namespace nacPDFEditor.models;

public class MainWindow : nac.ViewModelBase.ViewModelBase
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

    public byte[] ImagesLeftArrow { get; private set; }
    public byte[] ImagesRightArrow { get; private set; }
    public byte[] ImagesRotateLeft { get; private set; }
    public byte[] ImagesRotateRight { get; private set; }

    public MainWindow()
    {
        this.ImagesLeftArrow = lib.resources.GetImage("nacPDFEditor.resources.leftArrow.png");
        this.ImagesRightArrow = lib.resources.GetImage("nacPDFEditor.resources.rightArrow.png");
        this.ImagesRotateLeft = lib.resources.GetImage("nacPDFEditor.resources.rotateLeft.png");
        this.ImagesRotateRight = lib.resources.GetImage("nacPDFEditor.resources.rotateRight.png");
    }
}