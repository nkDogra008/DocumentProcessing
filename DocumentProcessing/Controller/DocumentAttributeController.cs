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
        /// <param name="AttributeId"></param>Represents the Id for each document type(eg: Invoice,Aadhaar etc)
        /// <returns></returns>
        public List<DocumentAttributes> GetAttributesById(int AttributeId)
        {
            List<DocumentAttributes> documentAttributes = null;
            try
            {
                if (AttributeId > 0)
                {
                    DocumentAttributeModel attributeModel = new DocumentAttributeModel();
                    documentAttributes = attributeModel.GetAttributesById(AttributeId);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return documentAttributes;
        }//GetAttributesById

        /// <summary>
        /// This method gets all Attributes
        /// </summary>
        /// <returns></returns>
        public List<DocumentAttributes> getAllAttributes()
        {
            List<DocumentAttributes> documentAttributes = null;
            try
            {
                
                    DocumentAttributeModel attributeModel = new DocumentAttributeModel();
                    documentAttributes = attributeModel.getAllAttributes();
              
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return documentAttributes;
        }//getAllAttributes

    }//DocumentAttributeController
}
