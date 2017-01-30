using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DocumentProcessing.Utility
{
    /// <summary>
    /// Date:19/01/2017
    /// This class maintains the error log and sends the email notification if error occurs in a document
    /// test
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Gets and Sets the path where log file needs to be saved
        /// </summary>
        public string LogFilePath { get; set; }



        /// <summary>
        /// This method logs the file and sends the notification
        /// </summary>
        /// <param name="logType">Specifies the type of log(e.g. Error, Success, Application Error)</param>
        /// <param name="errorMsg">Specifies the error message to be logged</param>
        /// <param name="sendNotification">Sets whether to send notification mail or not (Optional Parameter)</param>
        /// <param name="emailId">Contains the recipient email id (Optional Parameter)</param>
        public static void FileLog(Common.LogType logType, string errorMsg, bool sendNotification = false, string emailId = "")
        {
            //Implementation pending
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

}
