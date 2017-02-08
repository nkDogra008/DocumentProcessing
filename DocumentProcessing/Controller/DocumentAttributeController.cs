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
    /// To approach document attribute controller
    /// </summary>
    class DocumentAttributeController : Log
    {
        public DocumentAttributeController()
        {

        }//DocumentAttributeController

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AttributeId"></param>
        /// <returns></returns>
        public List<DocumentAttributes> getAttributesList(int AttributeId)
        {
            List<DocumentAttributes> documentAttributes = null;
            try
            {
                if (AttributeId > 0)
                {
                    DocumentAttributeModel attributeModel = new DocumentAttributeModel();
                    documentAttributes = attributeModel.getAttributesList(AttributeId);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return documentAttributes;
        }
    }//DocumentAttributeController
}
