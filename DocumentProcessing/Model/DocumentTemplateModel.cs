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
        /// <summary>
        /// Inherit factory object from sqlFactory class and get all DocumentTemplate related data from database
        /// </summary>
        public DocumentTemplateModel()
        {

        }//DocumentTemplateModel

        /// <summary>
        /// This method returns rows from Docuemnt template table filtered by OcrId
        /// </summary>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        public List<DocumentTemplate> GetDocTemplateByType(Common.OcrType ocrType)
        {
            //List is created to store DocTemplate values
            List<DocumentTemplate> listDocTemplate = new List<DocumentTemplate>();
            IDataReader reader;
            DocumentTemplate doctemplate = null;
            try
            {
                //StoredProcedure name initialization
                string spName = "sp_getDocumentTemplateDetails";
                //Getting storedprocedure from DB and storing in DbCommand variable
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
                //Adding parameter in DB
                _dbConnection.AddInParameter(dbCommand, "OcrId", DbType.Int32, (int)ocrType);
                //Reader is used to execuit dbCommand
                using (reader = _dbConnection.ExecuteReader(dbCommand))
                {
                    //Loop is used to get values from db and add into list
                    while (reader.Read())
                    {
                        doctemplate = new DocumentTemplate();
                        doctemplate.DocTemplateId = reader.GetInt32(reader.GetOrdinal("Id"));
                        doctemplate.OcrTypeId = (Common.OcrType)reader.GetInt32(reader.GetOrdinal("OcrId"));
                        // doctemplate.DocTypeId = reader.GetInt32(reader.GetOrdinal("MetadataTypeId"));
                        //  doctemplate.A_Id = reader.GetInt32(reader.GetOrdinal("AId"));
                        doctemplate.LineNo = reader.GetInt32(reader.GetOrdinal("LineNumber"));
                        listDocTemplate.Add(doctemplate);
                    }
                }
            }
            //Error handling
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());

            }
            //Returns the list
            return listDocTemplate;
        }//GetDocTemplateByType
    }//DocumentTemplateModel
}
