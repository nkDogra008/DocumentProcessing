using DocumentProcessing.Utility;
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
        public DocumentTemplateModel()
        {

        }//DocumentTemplateModel

        /// <summary>
        /// GetDocTemplateByType
        /// </summary>
        /// <param name="docType"></param>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(int docType, Common.OcrType ocrType)
        {
            List<DocumentTemplate> DocTemplate = null;
            IDataReader reader;
            DocumentTemplate doctemplate;
            try
            {
                string spName = "sp_getDocTemplateDetails";
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
                using (reader = _dbConnection.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        doctemplate = new DocumentTemplate();
                        doctemplate.DocTemplateId = reader.GetInt32(reader.GetOrdinal("ID"));
                        doctemplate.OcrTypeId = (Common.OcrType)reader.GetInt32(reader.GetOrdinal("OcrID"));
                        doctemplate.AttributeId = reader.GetInt32(reader.GetOrdinal("AttributeID"));
                        doctemplate.LineNo = reader.GetInt32(reader.GetOrdinal("LineNumber"));
                        DocTemplate.Add(doctemplate);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());

            }
            return DocTemplate;
        }


        //Implementation pending

    }//DocumentTemplateModel
}
