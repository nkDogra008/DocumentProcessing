
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using DocumentProcessing.View;
using DocumentProcessing.Utility;
using System.Data;
using System.Data.Common;
using System;

namespace DocumentProcessing.Model
{
    /// <summary>
    /// 
    /// </summary>
    class MetadataModel : SqlFactory
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetadataModel()
        {

        }
        /// <summary>
        /// This method returns rows from Metadata table filtered by MetadataTypeId
        /// </summary>
        /// <param name="OcrId"></param>
        /// <returns></returns>
        public List<Metadata> getMetadataByTypeId(int MetadataTypeId)
        {
            List<Metadata> listMetadata = new List<Metadata>();
            Metadata metadata;
            IDataReader reader;
           
            string spName = "sp_getMetadataDetails";
            DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
            _dbConnection.AddInParameter(dbCommand, "MetadataTypeId", DbType.Int32, MetadataTypeId);
            using (reader = _dbConnection.ExecuteReader(dbCommand))
            {
                while (reader.Read())
                {
                    metadata = new Metadata();
                    metadata.MetadataId = reader.GetInt32(reader.GetOrdinal("MetadataId"));
                    metadata.Type = reader.GetString(reader.GetOrdinal("Type"));
                    metadata.Format = reader.GetString(reader.GetOrdinal("Format"));
                    metadata.AttributeName = reader.GetString(reader.GetOrdinal("AttributeName"));
                    listMetadata.Add(metadata);
                    
                }

            }
            return listMetadata;
        }//listMetadata
    }//MetadataModel
}
