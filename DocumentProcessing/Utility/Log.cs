using System;
using System.IO;

/// <summary>
/// Utility file
/// </summary>

namespace DocumentProcessing.Utility
{
    /// <summary>
    /// Date:19/01/2017
    /// This class maintains the error log and sends the email notification if error occurs in a document

    /// </summary>
    public class Log
    {
        /// <summary>
        /// Gets and Sets the path where log file needs to be saved
        /// </summary>
        public static string LogFilePath { get; set; }

        /// <summary>
        /// This method logs the file and sends the notification
        /// </summary>
        /// <param name="logType">Specifies the type of log(e.g. Error, Success, Application Error)</param>
        /// <param name="Msg">Specifies the error message to be logged</param>
        /// <param name="sendNotification">Sets whether to send notification mail or not (Optional Parameter)</param>
        /// <param name="emailId">Contains the recipient email id (Optional Parameter)</param>
        public static void FileLog(Common.LogType logType, string msg)
        {
            //Implementation
            //By using below code we can append the text if logfile is present else it will create.
            //logfile will create everyday in a specific path, name as "Log File_yyyyMMdd" .

            string logFile = Properties.Resources.InitialFileName +
                DateTime.Now.ToString(Properties.Resources.DateFormat) +
                Properties.Resources.Extention;
            StreamWriter log = File.AppendText(Log.LogFilePath + logFile);
            string[] arrDetailsOfExp = msg.Split(':');
            // Write to the file:
            //First will check logType according to that will write Message to the Log file            
            if (logType == Common.LogType.Error)       //if any Error will arise.
            {
                if (arrDetailsOfExp.Length >= 4)
                {
                    //ERROR:Message Details:Exception.message. Class Name : Line No 
                    log.WriteLine(logType + Properties.Resources.StringSeparator +
                        Properties.Resources.LogMessageDetails + arrDetailsOfExp[0] + Properties.Resources.ErrorDetailsSeperator + arrDetailsOfExp[1] +
                        Properties.Resources.ErrorDetailsSeperator + arrDetailsOfExp[2] + Properties.Resources.ErrorDetailsSeperator + arrDetailsOfExp[3]);

                }
            }
            //if code successfully execuited
            else if (logType == Common.LogType.Success)
                //if code successfully execuited
                log.WriteLine(logType + Properties.Resources.StringSeparator +
                    Properties.Resources.LogMessageDetails + msg);
            else
                log.WriteLine(logType + Properties.Resources.StringSeparator +
                    Properties.Resources.ApplicationErrorMessage + msg);
            log.WriteLine();
            log.WriteLine(Properties.Resources.Endline+Properties.Resources.ErrorDetailsSeperator+DateTime.Now);
            log.WriteLine();
            log.Close();
        }//FileLog
        /// <summary>
        /// This method sends mail notification
        /// </summary>
        /// <returns></returns>
        private bool SendMailNotification()
        {

            return false;
            //Implementation Pending 
        }//SendMailNotification

    }//Log

    //this implimentation defines to find Line number
}//Log


