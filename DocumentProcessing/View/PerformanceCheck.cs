using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    public class PerformanceCheck
    {
        /// <summary>
        /// Store match percentage
        /// </summary>
        public double MatchPercentage { get; set; }

        /// <summary>
        /// Gets and Sets image dpi 
        /// </summary>
        public int dpi { get; set; }

        /// <summary>
        ///  Gets and sets the format of file downloaded from email.
        ///  For example: Pan Card, Invoice and Passport
        /// </summary>
        public string DocumentFormat { get; set; }

        /// <summary>
        /// File Extention 
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Type of OCR (Abbyy Or Datacap)
        /// </summary>
        public Common.OcrType OCR { get; set; }

    }//PerformanceCheck
}
