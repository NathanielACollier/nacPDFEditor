using repos = nacPDFEditor.repos;

using nac.Forms;

var form = Avalonia.AppBuilder.Configure<nac.Forms.App>()
    .NewForm();

nac.Forms.lib.Log.OnNewMessage += (_s, _logEntry) =>
{
    string line = $"[{_logEntry.EventDate:hh_mm_tt}] - {_logEntry.Level} - {_logEntry.CallingMemberName} - {_logEntry.Message}";
    System.Diagnostics.Debug.WriteLine(line);
};
    
form.Title = "Nac PDF Editor";

await repos.MainWindow.run(form);