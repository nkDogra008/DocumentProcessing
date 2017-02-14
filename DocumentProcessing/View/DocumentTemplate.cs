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
        /// 
        /// </summary>
        public DocumentTemplate()
        {

        }//DocumentTemplate

        /// <summary>
        /// 
        /// </summary>
        public int DocTemplateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Common.OcrType OcrTypeId { get; set; }

        /// <summary>
        /// 
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
