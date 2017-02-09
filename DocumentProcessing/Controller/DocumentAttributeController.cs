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
        /// This method gets the required attributes depending upon document type(eg:Invoice,Aadhar etc)
        /// </summary>
        /// <param name="documentType"></param>Type of the document(eg:Invoice,Aadhar etc)
        /// <returns></returns>
        public List<DocumentAttributes> getAttributesList(string documentType)
        {
            List<DocumentAttributes> documentAttributes = null;
            try
            {
                if (documentType != null)
                {
                    DocumentAttributeModel attributeModel = new DocumentAttributeModel();
                    documentAttributes = attributeModel.getAttributesList(documentType);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return documentAttributes;
        }//getAttributesList

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
