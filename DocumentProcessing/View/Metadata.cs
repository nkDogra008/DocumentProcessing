using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// 
    /// </summary>
    class Metadata: CommonData
    {
        /// <summary>
        /// 
        /// </summary>
        public Metadata()
        {

        }//Metadata

        /// <summary>
        /// Unique Id for each Metadata
        /// </summary>
        public int MetadataId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MetadataTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AttributeId { get; set; }

    }//Metadata
}