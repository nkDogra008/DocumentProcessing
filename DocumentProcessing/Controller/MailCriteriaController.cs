using DocumentProcessing.Model;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// To interact with MailCriteriaModel and MailCriteria
    /// </summary>
    class MailCriteriaController : Log
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MailCriteriaController()
        {

        }//MailCriteriaController

        /// <summary>
        /// This method gets Mail Search Criteria Details
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetMailSearchCriteria()
        {
            Dictionary<string, List<string>> dictMailSearchCriteria = null;

            try
            {
                MailCriteriaModel mailCriteriaModel = new MailCriteriaModel();
                dictMailSearchCriteria = mailCriteriaModel.GetMailSearchCriteria();

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return dictMailSearchCriteria;
        }//GetMailSearchCriteria

    }//MailCriteriaController
}
