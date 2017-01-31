using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// To get or set different types of document attributes
    /// </summary>
    class DocumentAttributes
    {
        public DocumentAttributes()
        {

        }//DocumentAttributes

        /// <summary>
        /// Gets and sets document attribute
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets Attribute Id of specific attribute
        /// </summary>
        public int AttributeId { get; set; }



    }//DocumentAttributes
}
