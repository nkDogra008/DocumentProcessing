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
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DocumentAttributes()
        {

        }//DocumentAttributes

        /// <summary>
        /// Gets and sets document attribute name
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets and sets Attribute Id of specific document
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Gets and sets id of specific attribute
        /// </summary>
        public int A_Id { get; set; }

    }//DocumentAttributes
}
