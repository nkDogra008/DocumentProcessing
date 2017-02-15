using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    public class DocumentTemplate
    {

        /// <summary>
        /// To get or set template details for the documents 
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
        public int DocTypeId { get; set; }

        /// <summary>
        /// Gets Attribute Id of specific attribute
        /// </summary>
        public int A_Id { get; set; }

        /// <summary>
        /// To get and set specific row in text file
        /// </summary>
        public int LineNo { get; set; }

    }//DocumentTemplate
}
