using DocumentProcessing.Utility;
using DocumentProcessing.View;
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
    /// 
    /// </summary>
    class MetadataModel : SqlFactory
    {
        public MetadataModel()
        {

        }//MetadataModel

        public List<Metadata> GetMetadataByDocTypeId(int docTypeId)
        {
            List<Metadata> listMetadata = null;
            IDataReader reader;
            Metadata metadata;
            try
            {
               
                string spName = "sp_getMetadata";
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);

                using (reader = _dbConnection.ExecuteReader(dbCommand))

                {
                    while (reader.Read())
                    {
                        metadata = new Metadata();
                        metadata.MetadataId = reader.GetInt32(reader.GetOrdinal("Id"));
                        metadata.Type = reader.GetString(1);
                        metadata.Format = reader.GetString(2);
                        listMetadata.Add(metadata);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listMetadata;

        }
    }//MetadataModel
}
