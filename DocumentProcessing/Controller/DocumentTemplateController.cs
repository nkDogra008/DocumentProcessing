﻿using DocumentProcessing.Model;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentTemplateController
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DocumentTemplateController()
        {

        }//DocumentTemplateController

        /// <summary>
        /// This method returns rows from Docuemnt template table filtered by OcrId
        /// </summary>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(Common.OcrType ocrType)
        {
            List<DocumentTemplate> docTemplateList = null;
            try
            {
                if (ocrType > 0)
                {
                    DocumentTemplateModel documentTemplateModel = new DocumentTemplateModel();
                    docTemplateList=documentTemplateModel.GetDocTemplateByType(ocrType);
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.Message);
            }
            return docTemplateList;
        }//GetDocTemplateByType

    }//DocumentTemplateController
}
