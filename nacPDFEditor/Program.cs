using repos = nacPDFEditor.repos;
using log = nacPDFEditor.lib.log;

using nac.Forms;

var form = nac.Forms.Form.NewForm();

nac.Forms.lib.Log.OnNewMessage += (_s, _logEntry) =>
{
    log.write(_logEntry);
};
    
form.Title = "Nac PDF Editor";

log.info("Application started");

await repos.MainWindow.run(form);