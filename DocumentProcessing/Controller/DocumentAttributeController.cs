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
    /// To interact with DocumentAttributeModel and DocumentAtributes
    /// </summary>
    class DocumentAttributeController : Log
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DocumentAttributeController()
        {

        }//DocumentAttributeController

        /// <summary>
        /// This method gets the required attributes depending upon Attribute Id
        /// </summary>
        /// <param name="metadataTypeId"></param>Represents the Id for each document type(eg: Invoice,Aadhaar etc)
        /// <returns></returns>
        public List<DocumentAttributes> GetAttributesById(int metadataTypeId)
        {
            List<DocumentAttributes> documentAttributes = null;
            try
            {
                if (metadataTypeId > 0)
                {
                    DocumentAttributeModel attributeModel = new DocumentAttributeModel();
                    documentAttributes = attributeModel.GetAttributesById(metadataTypeId);
                }
                else
                    Log.FileLog(Common.LogType.ApplicationError, "Unable to find any records for metadata Type Id: " + metadataTypeId);
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return documentAttributes;
        }//GetAttributesById
    }//DocumentAttributeController
}
