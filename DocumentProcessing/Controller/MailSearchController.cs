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
        /// <summary>
        /// 
        /// </summary>
        public string PhraseToSearch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string senderEmail { get; set; }
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
            FindItemsResults<Item> dateResult = null;

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

                    string mailsearch = PhraseToSearch;
                    //string fromSender = senderEmail;
                    //Subject filter criteria
                    //A local variable subjectFilter stores the subject filter pattern passed from the database
                    SearchFilter.ContainsSubstring subjectFilter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, mailsearch, ContainmentMode.Substring, ComparisonMode.IgnoreCase);

                    //Mail body filter criteria
                    //A local variable bodytFilter stores the body filter pattern passed from the database
                    SearchFilter.ContainsSubstring bodyFilter = new SearchFilter.ContainsSubstring(ItemSchema.Body, mailsearch, ContainmentMode.Substring, ComparisonMode.IgnoreCase);
                    //Checks the search condition passed from the xml file
                    if (Equals(condition, "OR") && (subjectFilter.Value != string.Empty || bodyFilter.Value != string.Empty))
                    {
                        //Logical OR condition for pattern search in subject or body is stored in a local variable
                        SearchFilter.SearchFilterCollection orFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, subjectFilter, bodyFilter);

                        //The mails satisfying the search criteria are stored in a variable
                        searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, orFilter, new ItemView(10));
                    }
                    else
                    {
                        //Logical AND condition for pattern search in subject and body is stored in a local variable
                        SearchFilter.SearchFilterCollection andFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, subjectFilter, bodyFilter);
                        searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, andFilter, new ItemView(10));
                    }

                    //Variable stores today's date and time
                    DateTime date = DateTime.Today;

                    //Iterating through each item of SearchResult
                    foreach (Item item in searchResult.Items)
                    {
                        //Variable stores the sender name
                        string sender = ((EmailMessage)(item)).Sender.Name;

                        //Condition checks for the sender's name
                        if (sender == "Rath, Saswat" || sender == "Kumar, Narender")
                        {
                            //Filters the date of the received mail
                            SearchFilter dateFilter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, item.DateTimeReceived);

                            //Variable stores the date and time of the mail received 
                            DateTime searchDate = item.DateTimeReceived;

                            //Filter mails acording to today's date
                            if (searchDate.Day == date.Day)
                            {
                                //Variable stores the filtered emails
                                dateResult = exchangeService.FindItems(WellKnownFolderName.Inbox, dateFilter, new ItemView(500));
                            }

                        }

                    }
                    //Logs success message into log file
                    Log.FileLog(Common.LogType.Success, "MailSearchCriteria : Total No. of mails found:" + searchResult.TotalCount);
                }
                              
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return dateResult;
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
