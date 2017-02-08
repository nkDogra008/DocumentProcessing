using DocumentProcessing.Model;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// 
    /// </summary>
    class MailCriteriaController
    {
        /// <summary>
        /// 
        /// </summary>
        public MailCriteriaController()
        {

        }//MailCriteriaController

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MailCriteria> GetAllMailCriteriaDetails()
        {
            List<MailCriteria> listMailCriteria = null;
            try
            {
                MailCriteriaModel mailCriteriaModel = new MailCriteriaModel();
                listMailCriteria = mailCriteriaModel.GetAllMailCriteriaDetails();
            }
            catch (Exception ex)
            {
            }
            return listMailCriteria;
        }//GetAllMailSearchDetails

        public Dictionary<string, string> GetMailSearchCriteria()
        {
            Dictionary<string, string> dictMailSearchCriteria = null;
            try
            {
                MailCriteriaModel mailCriteriaModel = new MailCriteriaModel();
                dictMailSearchCriteria = mailCriteriaModel.GetMailSearchCriteria();
            }
            catch (Exception ex)
            {

            }
            return dictMailSearchCriteria;
        }




    }//MailCriteriaController
}
