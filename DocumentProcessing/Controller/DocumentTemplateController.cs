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
    public class DocumentTemplateController
    {
        public DocumentTemplateController()
        {

        }//DocumentTemplateController

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(Common.OcrType ocrType)
        {
            List<DocumentTemplate> docTemplateList = null;
            try
            {
                if ((int)ocrType > 0)
                {
                    DocumentTemplateModel documentTemlateModel = new DocumentTemplateModel();
                    documentTemlateModel.GetDocTemplateByType(ocrType);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.Message);
            }
            return docTemplateList;
        }

    }//DocumentTemplateController
}
