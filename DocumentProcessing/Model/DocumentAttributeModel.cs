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
        /// This method gets all required Attributes based upon document type(eg invoice,Passport etc)
        /// </summary>
        /// <param name="documentType"></param>Type of the document(eg:Invoice,Aadhar etc)
        /// <returns></returns>
        public List<DocumentAttributes> getAttributesList(string documentType)
        {
            List<DocumentAttributes> listAttributes = new List<DocumentAttributes>();
            DocumentAttributes attributes;
            IDataReader reader;

            string spName = "sp_getAttributesList";
            DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
            _dbConnection.AddInParameter(dbCommand, "Name", DbType.String, documentType);
            using (reader = _dbConnection.ExecuteReader(dbCommand))
            {
                while (reader.Read())
                {
                    attributes = new DocumentAttributes();
                    attributes.AttributeName = reader.GetString(reader.GetOrdinal("AttributeName"));
                    listAttributes.Add(attributes);
                }
            }
            return listAttributes;
        }//getAttributesList(string metadataName)

        /// <summary>
        /// This method gets all attributes
        /// </summary>
        /// <returns></returns>
        public List<DocumentAttributes> getAllAttributes()
        {
            List<DocumentAttributes> listAttributes = new List<DocumentAttributes>();
            DocumentAttributes attributes;
            IDataReader reader;

            string spName = "sp_getAllAttributes";
            DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
            using (reader = _dbConnection.ExecuteReader(dbCommand))
            {
                while (reader.Read())
                {
                    attributes = new DocumentAttributes();
                    attributes.AttributeId = reader.GetInt32(reader.GetOrdinal("AId"));
                    attributes.AttributeId= reader.GetInt32(reader.GetOrdinal("AttributeId")); 
                    attributes.AttributeName = reader.GetString(reader.GetOrdinal("AttributeName"));
                    listAttributes.Add(attributes);
                }
            }
            return listAttributes;
        }//getAllAttributes()

    }//DocumentAttributeModel
}
