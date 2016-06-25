using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mz.betainteractive.utilities.module.General {
    public class LogErrors {

        public static void AddErrorLog(Exception ex, string title) {
            //string msg = ex.Message;
            //string stkTrace = ex.StackTrace;

            if (!(System.IO.Directory.Exists(Application.StartupPath + "\\Logs\\"))) {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Logs\\");
            }

            FileStream fs = new FileStream(Application.StartupPath + "\\Logs\\error.log", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter s = new StreamWriter(fs);
            s.Close();
            fs.Close();

            FileStream fs1 = new FileStream(Application.StartupPath + "\\Logs\\error.log", FileMode.Append, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(fs1);

            string msg = (ex == null) ? "" : ex.Message;
            string exception = (ex == null) ? "No Exception" : ex.ToString();

            s1.Write("Date/Time: " + DateTime.Now.ToString() + System.Environment.NewLine);
            s1.Write("Title: " + title + System.Environment.NewLine);
            s1.Write("Message: " + msg + System.Environment.NewLine);
            s1.Write("Exception Description: " + System.Environment.NewLine + exception + System.Environment.NewLine);            
            s1.Write("===========================================================================================\n" + System.Environment.NewLine);
            s1.Close();
            fs1.Close();
        }

    }
}
