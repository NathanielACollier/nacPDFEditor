using Avalonia.Media;
using nac.Forms.model;

namespace nacPDFEditor.repos;

public static class ErrorHandlerWindow
{
    public static async Task display(nac.Forms.Form f, Exception ex, string message="")
    {
        await f.DisplayChildForm(errorForm =>
        {
            errorForm.Title = $"Error with: {message}";

            errorForm.Model["details"] = ex.ToString();

            errorForm
                .Text("Message:")
                .Text(textToDisplay: message, style: new Style{foregroundColor = Colors.Blue})
                .Text("Exception Details")
                .TextBoxFor(modelFieldName: "details", style: new Style {foregroundColor = Colors.Red},
                    multiline: true, isReadOnly:true);

        }, useIsolatedModelForThisChildForm: true, isDialog: true);
    }
}