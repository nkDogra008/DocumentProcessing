﻿using DocumentProcessing.Utility;
using DocumentProcessing.View;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Model
{
    /// <summary>
    /// To get and store data related to extraction 
    /// </summary>
    class DocumentTemplateModel : SqlFactory
    {
        /// <summary>
        /// Inherit factory object from sqlFactory class and get all DocumentTemplate related data from database
        /// </summary>
        public DocumentTemplateModel()
        {

        }//DocumentTemplateModel

        /// <summary>
        /// GetDocTemplateByType
        /// </summary>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(Common.OcrType ocrType)
        {
            List<DocumentTemplate> listDocTemplate = null;
            IDataReader reader;
            DocumentTemplate doctemplate;
            try
            {
                string spName = "sp_getDocTemplateDetails";
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
                _dbConnection.AddInParameter(dbCommand, "OcrId", DbType.String, ocrType);
                using (reader = _dbConnection.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        doctemplate = new DocumentTemplate();
                        doctemplate.DocTemplateId = reader.GetInt32(reader.GetOrdinal("Id"));
                        doctemplate.OcrTypeId = (Common.OcrType)reader.GetInt32(reader.GetOrdinal("OcrId"));
                        doctemplate.DocTypeId = reader.GetInt32(reader.GetOrdinal("MetadataTypeId"));
                        doctemplate.A_Id = reader.GetInt32(reader.GetOrdinal("AId"));
                        doctemplate.LineNo = reader.GetInt32(reader.GetOrdinal("LineNumber"));
                        listDocTemplate.Add(doctemplate);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());

            }
            return listDocTemplate;
        }//listDocTemplate
    }//DocumentTemplateModel
}
