using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// To get or set template details for the documents 
    /// </summary>
    public class DocumentTemplate
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DocumentTemplate()
        {

        }//DocumentTemplate

        /// <summary>
        /// Gets and sets unique id for each document template
        /// </summary>
        public int DocTemplateId { get; set; }

        /// <summary>
        /// Gets and sets Id for each Ocr
        /// </summary>
        public Common.OcrType OcrTypeId { get; set; }

        /// <summary>
        /// Gets and sets unique Id for each document type(eg: Pan,Invoice etc.)
        /// </summary>
        public int metadataTypeId { get; set; }

        /// <summary>
        /// Gets and sets Id of specific attribute
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// To get and set specific row in text file
        /// </summary>
        public int LineNo { get; set; }

    }//DocumentTemplate
}
