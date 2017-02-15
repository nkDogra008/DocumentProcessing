﻿
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
    /// To manage Metadata
    /// </summary>
    class MetadataModel : SqlFactory
    {
        /// <summary>
        /// Inherit factory object from SqlFactory class and get all metadata related data from database
        /// Default Constructor
        /// </summary>
        public MetadataModel()
        {

        }//MetadataModel

        /// <summary>
        /// This method returns all rows from Metadata table
        /// </summary>
        /// <param name="MetadataTypeId"></param>
        /// <returns></returns>
        public List<Metadata> GetAllMetadataDetails()
        {
            List<Metadata> listMetadata = new List<Metadata>();
            Metadata metadata;
            IDataReader reader;

            string spName = "sp_getAllMetadataDetails";
            DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
            using (reader = _dbConnection.ExecuteReader(dbCommand))
            {
                while (reader.Read())
                {
                    metadata = new Metadata();
                    metadata.MetadataId = reader.GetInt32(reader.GetOrdinal("MetadataId"));
                    metadata.Type = reader.GetString(reader.GetOrdinal("Type"));
                    metadata.Format = reader.GetString(reader.GetOrdinal("Format"));
                    metadata.MetadataTypeId = reader.GetInt32(reader.GetOrdinal("MetadataTypeId"));
                    metadata.AttributeId=reader.GetInt32(reader.GetOrdinal("AttributeId"));
                    listMetadata.Add(metadata);
                }
            }
            return listMetadata;
        }//GetAllMetadataDetails

        /// <summary>
        /// This method returns rows from Metadata table filtered by MetadataTypeId
        /// </summary>
        /// <param name="MetadataTypeId"></param>Unique Id for each Document Type(eg Invoice,Aadhaar etc)
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
                   // metadata.AttributeName = reader.GetString(reader.GetOrdinal("AttributeName"));
                    listMetadata.Add(metadata);
                }
            }
            return listMetadata;
        }//getMetadataByTypeId
    }//MetadataModel
}
