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

            Dictionary<string, List<string>> dictSearchMailCriteria = null;
            MailCriteriaController mailCriteriaController = new MailCriteriaController();
            dictSearchMailCriteria = mailCriteriaController.GetMailSearchCriteria();
            string mailSearch = null;

            if (dictSearchMailCriteria != null)
                foreach (KeyValuePair<string, List<string>> keyValue in dictSearchMailCriteria)
                {
                    mailSearch = keyValue.Key;
                    if (mailSearch == "Subject")
                        subjectList = keyValue.Value;
                    else if (mailSearch == "FromEmail")
                        senderList = keyValue.Value;
                    else if (mailSearch == "Body")
                        bodyList = keyValue.Value;
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
            FindItemsResults<Item> filterResult = null;

            SearchFilter.ContainsSubstring subjectFilter = null;
            SearchFilter.ContainsSubstring bodyFilter = null;
            List<SearchFilter> SubjectArraySearchFilter = new List<SearchFilter>();
            List<SearchFilter> BodyArraySearchFilter = new List<SearchFilter>();
            try
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime date = DateTime.Today.AddDays(-1);
                SearchFilter.IsGreaterThanOrEqualTo filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(500));

                if (searchResult.Items.Count > 0)
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
                searchResult.Items.Clear();
                //  string mailsearch = PhraseToSearch;
                //Subject filter criteria
                //A local variable subjectFilter stores the subject filter pattern passed from the database
                foreach (string subjectCriteria in subjectList)
                {
                    subjectFilter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, subjectCriteria, ContainmentMode.Substring, ComparisonMode.IgnoreCase);
                    //SubjectArraySearchFilter.Add(subjectFilter);

                    //Mail body filter criteria
                    //A local variable bodytFilter stores the body filter pattern passed from the database
                    foreach (string bodyCriteria in bodyList)
                    {
                        bodyFilter = new SearchFilter.ContainsSubstring(ItemSchema.Body, bodyCriteria, ContainmentMode.Substring, ComparisonMode.IgnoreCase);
                        //BodyArraySearchFilter.Add(bodyFilter);
                        //Checks the search condition passed from the xml file
                        if (Equals(condition, "OR") && (subjectFilter.Value != string.Empty || bodyFilter.Value != string.Empty))
                        {
                            //Logical OR condition for pattern search in subject or body is stored in a local variable
                            SearchFilter.SearchFilterCollection orFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, subjectFilter, bodyFilter);
                            //The mails satisfying the search criteria are stored in a variable
                            filterResult = exchangeService.FindItems(WellKnownFolderName.SearchFolders, orFilter, new ItemView(1000));
                            foreach (var item in filterResult.Items)
                            {
                                if (searchResult.Items.Where(a => a.Id == item.Id).Count() <= 0)
                                    searchResult.Items.Add(item);
                            }
                        }
                        else
                        {
                            //Logical AND condition for pattern search in subject and body is stored in a local variable
                            SearchFilter.SearchFilterCollection andFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, subjectFilter, bodyFilter);
                            filterResult = exchangeService.FindItems(WellKnownFolderName.SearchFolders, andFilter, new ItemView(1000));
                            foreach (var item in filterResult.Items)
                            {
                                if (searchResult.Items.Where(a => a.Id == item.Id).Count() <= 0)
                                    searchResult.Items.Add(item);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return searchResult;
        }

        private void DownLoadattachment(outlook.MailItem eMail, string attachmentSavePath)
        {
            MetadataController metadataController = new MetadataController();
            List<Metadata> metadataList = metadataController.GetAllMetadataDetails();
            MailServerDetailController mailServerDetailController = new MailServerDetailController(attachmentSavePath);
            try
            {
                MailServerDetail mailserverDetail = mailServerDetailController.GetMailServerDetails;
                if (null != mailserverDetail)
                {

                    foreach (Metadata metadata in metadataList)
                    {
                        type = metadata.Type;
                        format = metadata.Format;

                        // Path where attachments will be saved
                        string filePath = attachmentSavePath;

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
                                string fileExtension = fileName.Split('.')[1];
                                //Checks for the extension in the attachments
                                extensionString = extensionString.Select(a => a.Replace('.', ' ').Trim().ToLower()).ToArray();
                                if (extensionString.Contains(fileExtension) && fileName.ToLower().Contains(type.ToLower()))
                                {
                                    //Saves the attachment into the filepath
                                    attachments[i].SaveAsFile(filePath + attachments[i].FileName);
                                }
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


        public void OutlookMailSearch(outlook.Application app, string attachmentPath)
        {

            try
            {

                string condition = null;
                // outlook.Application app = new outlook.Application();
                string scope = "Inbox";
                outlook.NameSpace outlookNamespace = app.GetNamespace("MAPI");
                outlook.MAPIFolder folderInbox = outlookNamespace.GetDefaultFolder(outlook.OlDefaultFolders.olFolderInbox);
                scope = "\'" + folderInbox.FolderPath + "\'";

                XmlDocument reader = new XmlDocument();
                reader.Load(@"D:\Project\SettingFile\Settings.xml");
                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("searchcondition")[0].InnerText))
                    condition = reader.GetElementsByTagName("searchcondition")[0].InnerText;

                DateTime date = DateTime.Today;

                outlook.Items items = folderInbox.Items;
                foreach (outlook.MailItem eMail in items.Restrict("[ReceivedTime] > '" + date.ToString("MM/dd/yyyy HH:mm") + "'"))
                {
                    string senderEmailAdd = string.Empty;
                    outlook.AddressEntry sender = eMail.Sender;
                    outlook.ExchangeUser exchUser = sender.GetExchangeUser();
                    if (null != exchUser)
                        senderEmailAdd = exchUser.PrimarySmtpAddress;
                    else
                        senderEmailAdd = eMail.SenderEmailAddress;
                    foreach (string subjectCriteria in subjectList)
                    {
                        foreach (string bodyCriteria in bodyList)
                        {
                            if (condition == "OR" && senderList.Contains(senderEmailAdd))
                            {

                                if ((eMail != null && eMail.Subject != null && eMail.Subject.ToLower().Contains(subjectCriteria.ToLower())) || (eMail.Body != null && eMail.Body.ToLower().Contains(bodyCriteria.ToLower())))
                                {
                                    DownLoadattachment(eMail, attachmentPath);
                                }
                            }
                            else if (condition == "AND" && senderList.Contains(senderEmailAdd))
                            {

                                if ((eMail != null && eMail.Subject != null && eMail.Subject.ToLower().Contains(subjectCriteria.ToLower())) && (eMail.Body != null && eMail.Body.ToLower().Contains(bodyCriteria.ToLower())))
                                {
                                    DownLoadattachment(eMail, attachmentPath);
                                }
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


        public FindItemsResults<Item> MailSearchCriteria2(string condition, ExchangeService exchangeService)
        {
            FindItemsResults<Item> searchResult = null;
            bool mailExists = false;
            List<SearchFilter> SubjectArraySearchFilter = new List<SearchFilter>();
            List<SearchFilter> BodyArraySearchFilter = new List<SearchFilter>();
            List<Item> removeItems = new List<Item>();
            try
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                DateTime date = DateTime.Today.AddDays(-1);
                SearchFilter.IsGreaterThanOrEqualTo filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, date);
                searchResult = exchangeService.FindItems(WellKnownFolderName.Inbox, filter, new ItemView(500));

                if (searchResult.Items.Count > 0)
                {
                    exchangeService.LoadPropertiesForItems(searchResult.Items, new PropertySet(EmailMessageSchema.Sender));

                    // Remove email from result where there is no match with sender email id
                    if (senderList.Count > 0)
                    {
                        foreach (Item item in searchResult.Items)
                        {

                            string sender = ((EmailMessage)(item)).Sender.Address;
                            if (!senderList.Contains(sender))
                            {
                                removeItems.Add(item);
                            }

                        }
                        foreach (var item in removeItems)
                        {
                            searchResult.Items.Remove(item);
                        }
                        removeItems.Clear();
                    }
                    if (searchResult.Items.Count > 0)
                    {
                        exchangeService.LoadPropertiesForItems(searchResult.Items, PropertySet.FirstClassProperties);
                        if (Equals(condition, "OR"))
                        {

                            foreach (Item itemMail in searchResult)
                            {
                                if (subjectList.Count > 0)
                                {

                                    foreach (string subject in subjectList)
                                    {
                                        if (((EmailMessage)(itemMail)).Subject.ToLower().Contains(subject.ToLower()))
                                            mailExists = true;

                                    }
                                }
                                if (!mailExists)
                                {
                                    foreach (string bodyKeyword in bodyList)
                                    {
                                        if (((EmailMessage)(itemMail)).Body.ToString().ToLower().Contains(bodyKeyword.ToLower()))
                                            mailExists = true;


                                    }
                                }
                                if (!mailExists)
                                    removeItems.Add(itemMail);
                            }

                        }
                        else
                        {

                            foreach (Item itemMail in searchResult)
                            {

                                if (subjectList.Count > 0)
                                {
                                    foreach (string subject in subjectList)
                                    {
                                        if (((EmailMessage)(itemMail)).Subject.ToLower().Contains(subject.ToLower()))
                                            mailExists = true;

                                    }
                                }
                                if (mailExists)
                                {
                                    mailExists = false;
                                    foreach (string bodyKeyword in bodyList)
                                    {
                                        if (((EmailMessage)(itemMail)).Body.ToString().ToLower().Contains(bodyKeyword.ToLower()))
                                            mailExists = true;
                                    }
                                }
                                if (!mailExists)
                                    removeItems.Add(itemMail);
                            }

                        }

                        foreach (var item in removeItems)
                        {
                            searchResult.Items.Remove(item);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return searchResult;
        }

    }


}//MailSearchController



