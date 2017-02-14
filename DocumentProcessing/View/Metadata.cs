using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    ///  To get or set different types of metadata
    ///  Inherits objects from Common class
    /// </summary>
    class Metadata : Common
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Metadata()
        {

        }//Metadata

        /// <summary>
        /// Gets and sets unique Id for each Metadata
        /// </summary>
        public int MetadataId { get; set; }

        /// <summary>
        /// Gets and sets document type(eg. Invoice,Aadhaar etc)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets and sets document format
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets and sets unique Id for each MetadataType
        /// </summary>
        public int MetadataTypeId { get; set; }

    }//Metadata
}