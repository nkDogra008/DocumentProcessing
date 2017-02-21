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
        /// Gets and sets unique Id for specific attribute
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// Gets and sets attribute name for each Id
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets and sets id of specific document (eg: Pan,Passport etc)
        /// </summary>
        public int MetadataTypeId { get; set; }


    }//DocumentAttributes
}
