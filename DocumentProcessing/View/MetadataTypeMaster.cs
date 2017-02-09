using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// To get or set metadata types
    /// </summary>
    class MetadataTypeMaster
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetadataTypeMaster()
        {

        }//MetadataTypeMaster

        /// <summary>
        /// Gets and sets unique id for each metadata type
        /// </summary>
        public int MetadataTypeId { get; set; }

        /// <summary>
        /// Gets and sets Name against each id
        /// </summary>
        public string Name { get; set; }

    }//MetadataTypeMaster
}

