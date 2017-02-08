using DocumentProcessing.Model;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using Microsoft.Exchange.WebServices.Data;
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
    class MailSearchController
    {
        public string PhraseToSearch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MailSearchController()
        {

        }//MailSearchController

        /// <summary>
        /// Class for mail search criteria which dynamically
        ///  takes the search pattern from the database 
        /// </summary>
        /// <param name="condition">The search condition is passed from the xml file</param>
        /// <param name="exchangeService">The exchange service class is passed from the main program</param>
        /// <returns></returns>
        public FindItemsResults<Item> MailSearchCriteria(string condition, ExchangeService exchangeService)
        {
            FindItemsResults<Item> searchResult = null;
            Dictionary<string, string> dictSearchMailCriteria = null;
            MailCriteriaController mailCriteriaController = new MailCriteriaController();


            try
            {
                //Timespan for which the mailbox is to be searched is provided
                //TimeSpan ts = new TimeSpan(0, 0, 0, 0);
                dictSearchMailCriteria = mailCriteriaController.GetMailSearchCriteria();


                ////Date for which the mailbox is to be searched is provided
                ////To be made configurable from XML file
                //DateTime date = DateTime.Now.AddDays(-1);

                ////A local variable filter stores the search condition according to the date
                //SearchFilter.IsGreaterThanOrEqualTo filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);

                ////All the mails for the given date is stored in a variable
                //searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(500));

                ////Count of all the mails with date filter is stored in an integer variable
                //int mailno = searchResult.Count();

                ////Runs the loop for the count to be greater than 0
                //while (mailno >= 0)
                //{
                //Subject filter criteria

                foreach (KeyValuePair<string, string> keyValue in dictSearchMailCriteria)
                {

                    //A local variable subjectFilter stores the subject filter pattern passed from the database
                    SearchFilter.ContainsSubstring subjectFilter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, keyValue.Key, ContainmentMode.Substring, ComparisonMode.IgnoreCase);

                    //Mail body filter criteria
                    //A local variable bodytFilter stores the body filter pattern passed from the database
                    SearchFilter.ContainsSubstring bodyFilter = new SearchFilter.ContainsSubstring(ItemSchema.Body, keyValue.Key, ContainmentMode.Substring, ComparisonMode.IgnoreCase);

                    //Checks the search condition passed from the xml file
                    if (Equals(condition, "OR") & (subjectFilter.Value != string.Empty || bodyFilter.Value != string.Empty))
                    {
                        //Logical OR condition for pattern search in subject or body is stored in a local variable
                        SearchFilter.SearchFilterCollection orFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, subjectFilter, bodyFilter);

                        //The mails satisfying the search criteria are stored in a variable
                        searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, orFilter, new ItemView(500));
                    }
                    else
                    {
                        //Logical AND condition for pattern search in subject and body is stored in a local variable
                        SearchFilter.SearchFilterCollection andFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, subjectFilter, bodyFilter);

                        //The mails satisfying the search criteria are stored in a variable
                        searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, andFilter, new ItemView(500));
                    }
                }
                //The count is decremented
                //    mailno--;
                //}
                // Log message for success (mail found after mail search criteria)
                Log.FileLog(Common.LogType.Success, "MailSearchCriteria : Total No. of mails found:" + searchResult.TotalCount);
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MailSearch> GetAllMailSearchDetails()
        {
            List<MailSearch> listMailSearch = new List<MailSearch>();
            try
            {
                MailSearchModel mailSearchModel = new MailSearchModel();
                listMailSearch = mailSearchModel.GetAllMailSearchDetails();
            }
            catch (Exception ex)
            {
            }
            return listMailSearch;
        }


    }//MailSearchController
}
