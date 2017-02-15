﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Utility
{
    /// <summary>
    /// To access common objects 
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
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

            Aspire = 1,
            Abbyy,
            OcrNotFound,
            Datacap,


        }//OcrTypes

        /// <summary>
        /// Gets and sets Unique Id depending upon the type of document(eg. Invoice,Passport etc)
        /// </summary>
        public int AttributeId { get; set; }

    }//Common
}
