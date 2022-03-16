namespace nacPDFEditor.repos;

public static class ErrorHandlerWindow
{
    public static async Task display(nac.Forms.Form f, Exception ex, string message="")
    {
        await f.DisplayChildForm(errorForm =>
        {
            errorForm.Title = $"Error with: {message}";

        }, useIsolatedModelForThisChildForm: true, isDialog: true);
    }
}