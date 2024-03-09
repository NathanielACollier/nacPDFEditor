using repos = nacPDFEditor.repos;


var log = new nac.Logging.Logger();
nac.Logging.Appenders.ColoredConsole.Setup();

var form = nac.Forms.Form.NewForm();


    
form.Title = "Nac PDF Editor";

log.Info("Application started");

await repos.MainWindow.run(form);