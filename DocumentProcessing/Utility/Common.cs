using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Utility
{
    public class Common
    {
        public Common()
        {

        }//Common

        /// <summary>
        /// Logs the type of error
        /// </summary>
        public enum LogType
        {
            Error = 0,
            Success,
            ApplicationError
        }//LogType

        public enum ServerType
        {
            ExchangeServer,
            Outlook,
            Other

        }//LogType



        /// <summary>
        /// Specifies the type of OCR
        /// </summary>
        public enum OcrType
        {
            // Can be used if no performance is available to compare
            OcrNotFound = 1,
            Datacap,
            Abbyy,
            Aspire

        }//OcrTypes
    }
}
