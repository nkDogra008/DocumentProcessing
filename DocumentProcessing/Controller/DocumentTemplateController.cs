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
        /// <param name="docType"></param>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(int docType, Common.OcrType ocrType)
        {
            List<DocumentTemplate> test = null;
            try
            {
                if (docType > 0 && ocrType > 0)
                {
                    DocumentTemplateModel documentTemlateModel = new DocumentTemplateModel();
                    documentTemlateModel.GetDocTemplateByType(docType, ocrType);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.Message);
            }
            return test;
        }

    }//DocumentTemplateController
}
