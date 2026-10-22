using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mz.betainteractive.utilities.module.General {
    public class LogErrors {

        public static void AddErrorLog(Exception ex, string title) {
            try {
                // 1. Move the path to Windows ProgramData
                string logFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "sigeas",
                    "Logs"
                );
                string logFilePath = Path.Combine(logFolder, "error.log");

                // 2. Safely ensure the directory exists
                if (!Directory.Exists(logFolder)) {
                    Directory.CreateDirectory(logFolder);
                }

                // 3. Format the exception details neatly
                string msg = ex?.Message ?? "No message";
                string exception = ex?.ToString() ?? "No Exception";

                // Use a System.Text.StringBuilder or a clean interpolation layout
                string logEntry =
                    $"Date/Time: {DateTime.Now}\r\n" +
                    $"Title: {title}\r\n" +
                    $"Message: {msg}\r\n" +
                    $"Exception Description: \r\n{exception}\r\n" +
                    $"===========================================================================================\r\n\r\n";

                // 4. Thread-safe, quick append operation (automatically handles opening/closing stream safely)
                File.AppendAllText(logFilePath, logEntry);
            } catch {
                // Fail-silent inside a logging framework to prevent crashing the app over a log failure
            }
        }

    }
}
