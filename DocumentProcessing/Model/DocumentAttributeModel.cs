using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Text;
using System.Collections.Generic;
using DocumentProcessing.View;
using DocumentProcessing.Utility;

namespace DocumentProcessing.Model
{
    /// <summary>
    /// To manage DocumentAttributes
    /// </summary>
    class DocumentAttributeModel : SqlFactory
    {
        /// <summary>
        /// Inherit factory object from SqlFactory class and get all attribute related data from database
        /// Default Constructor
        /// </summary>
        public DocumentAttributeModel()
        {

        }//DocumentAttributeModel

        /// <summary>
        /// This method gets all required Attributes based upon Attribute Id
        /// </summary>
        /// <param name="AttributeId"></param>Represents type Id of document(eg Invoice,passport etc)
        /// <returns></returns>
        public List<DocumentAttributes> GetAttributesById(int AttributeId)
        {
            List<DocumentAttributes> listAttributes = new List<DocumentAttributes>();
            DocumentAttributes attributes;
            IDataReader reader;
            try
            {
                string spName = "sp_getAttributesById";
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
                _dbConnection.AddInParameter(dbCommand, "AttributeId", DbType.Int32, AttributeId);
                using (reader = _dbConnection.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        attributes = new DocumentAttributes();
                        attributes.A_Id = reader.GetInt32(reader.GetOrdinal("AId"));
                        attributes.AttributeName = reader.GetString(reader.GetOrdinal("AttributeName"));
                        listAttributes.Add(attributes);
                    }
                }
            }
            //Error Handling
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return listAttributes;
        }//GetAttributesById
    }//DocumentAttributeModel
}
