using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DocumentProcessing.Utility.Common;

namespace DocumentProcessing.View
{
    /// <summary>
    /// Prebuild table for extracting from document
    /// </summary>
    class NamedSearchAttributes
    {
        /// <summary>
        /// To get and set type of ocr (Abbyy or Datacap) 
        /// </summary>
        public OcrType OcrNumber { get; set; }

        /// <summary>
        /// To get and set NameSearchId 
        /// </summary>
        public int NameSearchId { get; set; }

        /// <summary>
        /// To get and set phrase (for which attribute we are checking)
        /// </summary>
        public string Phrase { get; set; }

        /// <summary>
        /// To get and set different phrase match (alternate keyword for a phrase)
        /// (Ex: For Address Phrase : we can have PhraseMatch : Add, Addr and Addre etc)
        /// </summary>
        public string PhraseMatch { get; set; }

    }//NamedSearchAttributes
}
