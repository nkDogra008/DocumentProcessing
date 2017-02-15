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
    class MailCriteriaModel : SqlFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public MailCriteriaModel()
        {

        }//MailCriteriaModel

        /// <summary>
        /// This method returns subject and criteria for filtering mails
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetMailSearchCriteria()
        {
            Dictionary<string, string> dictMailSearchCriteria = new Dictionary<string, string>();
            Dictionary<string, string> dictMetadata = new Dictionary<string, string>();
            IDataReader reader;
            string key = string.Empty, value = string.Empty;

            try
            {
                string spName = "sp_getMailSearchDetails ";
                DbCommand dbCommand = _dbConnection.GetStoredProcCommand(spName);
                using (reader = _dbConnection.ExecuteReader(dbCommand))
                {
                    while (reader.Read())
                    {
                        key = reader.GetString(reader.GetOrdinal("Criteria"));
                        value = reader.GetString(reader.GetOrdinal("Subject"));
                        dictMailSearchCriteria.Add(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return dictMailSearchCriteria;
        }//GetMailSearchCriteria

    }//MailCriteriaModel
}
