using DocumentProcessing.Model;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using outlook = Microsoft.Office.Interop.Outlook;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// 
    /// </summary>
    class MailSearchController
    {
        //private FindItemsResults<Item> searchResult;


        MailServerDetailController mailServerDetailController = new MailServerDetailController();
        private string type;
        private string format;
        List<string> subjectList = new List<string>();
        List<string> senderList = new List<string>();
        List<string> bodyList = new List<string>();

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

            Dictionary<string, string> dictSearchMailCriteria = null;
            MailCriteriaController mailCriteriaController = new MailCriteriaController();
            dictSearchMailCriteria = mailCriteriaController.GetMailSearchCriteria();
            string subject = null;
            string mailSearch = null;

            if (dictSearchMailCriteria != null)
                foreach (KeyValuePair<string, string> keyValue in dictSearchMailCriteria)
                {
                    subject = keyValue.Value;
                    mailSearch = keyValue.Key;
                    //PhraseToSearch = keyValue.Key;
                    if (subject == "Subject")
                    {
                        subjectList.Add(mailSearch);
                        //subjectList.Add(PhraseToSearch);

                    }
                    else if (subject == "FromEmail")
                    {
                        senderList.Add(mailSearch);
                        //senderList.Add(PhraseToSearch);
                    }
                    else
                        bodyList.Add(mailSearch);
                    //bodyList.Add(PhraseToSearch);
                }
        }//MailSearchController

        /// <summary>/// Class for mail search criteria which dynamically
        ///  takes the search pattern from the database 
        /// </summary>
        /// <param name="condition">The search condition is passed from the xml file</param>
        /// <param name="exchangeService">The exchange service class is passed from the main program</param>
        /// <returns></returns>
        public FindItemsResults<Item> MailSearchCriteria(string condition, ExchangeService exchangeService)
        {
            FindItemsResults<Item> searchResult = null;
            SearchFilter.ContainsSubstring subjectFilter = null;
            SearchFilter.ContainsSubstring bodyFilter = null;
            List<SearchFilter> testArraySearchFilter = new List<SearchFilter>();
            try
            {
                DateTime date = DateTime.Now.AddDays(-1);
                SearchFilter.IsGreaterThanOrEqualTo filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(500));
                exchangeService.LoadPropertiesForItems(searchResult.Items, new PropertySet(EmailMessageSchema.Sender));
                foreach (Item item in searchResult.Items)
                {
                    string sender = ((EmailMessage)(item)).Sender.Address;
                    foreach (string senderName in senderList)
                    {
                        if (senderName == sender)
                        {
                            item.Copy(WellKnownFolderName.SearchFolders);
                        }
                    }

                }
                //  string mailsearch = PhraseToSearch;
                //Subject filter criteria
                //A local variable subjectFilter stores the subject filter pattern passed from the database
                foreach (string subjectCriteria in subjectList)
                {
                    subjectFilter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, subjectCriteria, ContainmentMode.Substring, ComparisonMode.IgnoreCase);
                    testArraySearchFilter.Add(subjectFilter);
                }

                //Mail body filter criteria
                //A local variable bodytFilter stores the body filter pattern passed from the database
                foreach (string bodyCriteria in bodyList)
                {
                    bodyFilter = new SearchFilter.ContainsSubstring(ItemSchema.Body, bodyCriteria, ContainmentMode.Substring, ComparisonMode.IgnoreCase);
                    testArraySearchFilter.Add(bodyFilter);
                }
                //Checks the search condition passed from the xml file
                if (Equals(condition, "OR") && (subjectFilter.Value != string.Empty || bodyFilter.Value != string.Empty))
                {
                    //Logical OR condition for pattern search in subject or body is stored in a local variable
                    SearchFilter.SearchFilterCollection orFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, testArraySearchFilter.ToArray());

                    //The mails satisfying the search criteria are stored in a variable
                    searchResult = exchangeService.FindItems(WellKnownFolderName.SearchFolders, orFilter, new ItemView(1000));
                }
                else
                {
                    //Logical AND condition for pattern search in subject and body is stored in a local variable
                    SearchFilter.SearchFilterCollection andFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, testArraySearchFilter.ToArray());
                    searchResult = exchangeService.FindItems(WellKnownFolderName.SearchFolders, andFilter, new ItemView(1000));
                }

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return searchResult;
        }

        private void DownLoadattachment(outlook.MailItem eMail)
        {
            MetadataController metadataController = new MetadataController();
            List<Metadata> metadataList = metadataController.GetAllMetadataDetails();

            try
            {

                foreach (Metadata metadata in metadataList)
                {
                    type = metadata.Type;
                    format = metadata.Format;
                    XmlDocument reader = new XmlDocument();
                    //Loads the xml file into the instance
                    reader.Load("Settings.xml");

                    // Path where attachments will be saved
                    string filePath = reader.GetElementsByTagName("path")[0].InnerText;

                    //Attachment extensions to save are stored in an array
                    string[] extensionString = format.Split(',');


                    //Attachments are stored in a variable
                    var attachments = eMail.Attachments;


                    //Checks whether attachment count is not zero
                    if (attachments.Count != 0)
                    {
                        //Loops through each attachment of the mail
                        for (int i = 1; i < attachments.Count; i++)
                        {
                            string fileName = attachments[i].FileName;
                            //Checks for the extension in the attachments
                            if (extensionString.Any(attachments[i].FileName.Contains) && fileName.Contains(type))
                            {
                                //Saves the attachment into the filepath
                                attachments[i].SaveAsFile(filePath + attachments[i].FileName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }

        }
        public void OutlookMailSearch(string wordInSubject, outlook.Application app)
        {
            string condition = null;
            // outlook.Application app = new outlook.Application();
            string scope = "Inbox";
            outlook.NameSpace outlookNamespace = app.GetNamespace("MAPI");
            outlook.MAPIFolder folderInbox = outlookNamespace.GetDefaultFolder(outlook.OlDefaultFolders.olFolderInbox);
            scope = "\'" + folderInbox.FolderPath + "\'";

            XmlDocument reader = new XmlDocument();
            reader.Load("settings.xml");
            if (!string.IsNullOrEmpty(reader.GetElementsByTagName("searchcondition")[0].InnerText))
                condition = reader.GetElementsByTagName("searchcondition")[0].InnerText;

            DateTime date = DateTime.Today;

            outlook.Items items = folderInbox.Items;
            foreach (outlook.MailItem eMail in items.Restrict("[ReceivedTime] > '" + date.ToString("MM/dd/yyyy HH:mm") + "'"))
            {
                foreach (string subjectCriteria in subjectList)
                {
                    foreach (string bodyCriteria in bodyList)
                    {
                        if (condition == "OR" && senderList.Contains(eMail.SenderEmailAddress))
                        {

                            if ((eMail != null && eMail.Subject != null && eMail.Subject.Contains(subjectCriteria)) || (eMail.Body != null && eMail.Body.Contains(bodyCriteria)))
                            {
                                DownLoadattachment(eMail);
                            }
                        }
                        else if (condition == "AND" && senderList.Contains(eMail.SenderEmailAddress))
                        {

                            if ((eMail != null && eMail.Subject != null && eMail.Subject.Contains(subjectCriteria)) && eMail.Body.Contains(bodyCriteria))
                            {
                                DownLoadattachment(eMail);
                            }
                        }
                    }
                }
            }
        }
    }


}//MailSearchController



