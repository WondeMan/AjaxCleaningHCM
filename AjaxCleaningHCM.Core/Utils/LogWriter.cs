using System;
using System.IO;

namespace AjaxCleaningHCM.Core.Utils
{
    public class LogWriter
    {
        public void CreateLog(string strLogText, string strModuleName, string logName)
        {
            string strLogFileName = null;

            try
            {
                string strLOGDIR = @"C:\AjaxCleaningHCM\Log\";
                if (!Directory.Exists(strLOGDIR))
                {
                    Directory.CreateDirectory(strLOGDIR);
                }
                strLogFileName = strLOGDIR + logName + " " + DateTime.Now.ToString("dd-MMM-yyyy") + ".log";
                StreamWriter fileLogFileWriter = new StreamWriter(strLogFileName, true);
                strLogText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt") + " " + strLogText + " Module = " + strModuleName;
                fileLogFileWriter.WriteLine(strLogText);
                fileLogFileWriter.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
