using repos = nacPDFEditor.repos;

using nac.Forms;

var form = Avalonia.AppBuilder.Configure<nac.Forms.App>()
    .NewForm();
    
form.Title = "Nac PDF Editor";

await repos.MainWindow.run(form);