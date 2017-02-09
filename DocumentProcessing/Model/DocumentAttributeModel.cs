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
    /// To get and store attributes related data from database
    /// </summary>
    class DocumentAttributeModel : SqlFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public DocumentAttributeModel()
        {

        }//DocumentAttributeModel

        /// <summary>
        /// getAttributesList
        /// </summary>
        /// <param name="AttributeId"></param>
        /// <returns></returns>
        public List<DocumentAttributes> getAttributesList(int metadataName)
        {
            List<DocumentAttributes> listAttributes = new List<DocumentAttributes>();
            DocumentAttributes attributes;
            IDataReader reader;

            string spName = "sp_getAttributesList";
            DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
            _dbConnection.AddInParameter(dbCommand, "Name", DbType.String, metadataName);
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
        }

    }//DocumentAttributeModel
}
