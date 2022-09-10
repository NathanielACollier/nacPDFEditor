using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace nacPDFEditor.lib;

public static class log
{

    private static void writeLogRaw(string level, string message, string callingMemberInformation, DateTime? eventDate = null)
    {
        if( eventDate == null)
        {
            eventDate = DateTime.Now;
        }

        string line = $"[{eventDate:hh_mm_tt}] - {level} - {callingMemberInformation} - {message}";
        System.Diagnostics.Debug.WriteLine(line);
        System.Console.WriteLine(line);
    }


    public static void write(nac.Forms.lib.Log.LogMessage entry)
    {
        writeLogRaw(level: entry.Level,
            message: entry.Message,
            callingMemberInformation: entry.CallingMemberName,
            eventDate: entry.EventDate);
    }


    public static void info(string message, [CallerMemberName] string callerMemberName = "")
    {
        writeLogRaw(level: "INFO",
            message: message,
            callingMemberInformation: callerMemberName);
    }

}
