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
        /// This method returns rows from Metadata table filtered by MetadataTypeId
        /// </summary>
        /// <param name="MetadataTypeId"></param>Unique Id for each Document Type (eg Invoice,Aadhaar etc)
        /// <returns></returns>
        public List<Metadata> getMetadataByTypeId(int MetadataTypeId)
        {
            List<Metadata> listMetadata = null;
            try
            {
                if (MetadataTypeId > 0)
                {
                    MetadataModel metadataModel = new MetadataModel();
                    listMetadata = metadataModel.getMetadataByTypeId(MetadataTypeId);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return listMetadata;
        }

    }//MetadataController
}
