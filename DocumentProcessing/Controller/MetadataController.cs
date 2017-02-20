using DocumentProcessing.Model;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// To interact with MetadataModel and Metadata
    /// </summary>
    class MetadataController : Log
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetadataController()
        {

        }//MetadataController

        /// <summary>
        /// This method returns all rows from Metadata table
        /// </summary>
        /// <returns></returns>
        public List<Metadata> GetAllMetadataDetails()
        {
            List<Metadata> listMetadata = null;
            try
            {             
                    MetadataModel metadataModel = new MetadataModel();
                    listMetadata = metadataModel.GetAllMetadataDetails();               
            }
            //Error handling
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return listMetadata;
        }//GetAllMetadataDetails
    }//MetadataController
}
