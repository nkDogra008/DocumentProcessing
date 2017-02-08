using DocumentProcessing.Utility;
using DocumentProcessing.View;
using Microsoft.Exchange.WebServices.Data;
using Outlook = Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// To get mail server details
    /// </summary>
    public class MailServerDetailController
    {
        private MailServerDetail GetMailServerDetails { get; set; }
        private MailSearch GetMailSearchDetails { get; set; }
        private MailCriteria GetMailCriteria { get; set; }
        private string _mailSearchCondition = string.Empty;
        private string _saveAttachmentPath = string.Empty;
        private string _serverType = string.Empty;
        private string _phraseToSearch = string.Empty;

        /// <summary>
        /// This method controls the mail server details
        /// </summary>
        /// <param name="filePathToSaveAttachments">Path to save the attachment is passed as parameter</param>
        public MailServerDetailController(string filePathToSaveAttachments)
        {
            //Contructor will load all the details from XML file
            // Path of XML to be specified
            _saveAttachmentPath = filePathToSaveAttachments;
            SetMailServerDetails();
        }

        /// <summary>
        /// Sets the mail server details from the XML file
        /// </summary>
        private void SetMailServerDetails()
        {
            MailServerDetail mailServerDetail = new MailServerDetail();
            MailSearch mailSearchDetail = new MailSearch();
            MailCriteria mailCriteria = new MailCriteria();
            try
            {
                //Creates an instance of XmlDocument
                XmlDocument reader = new XmlDocument();

                //Loads the xml file into the instance
                reader.Load(@"D:\Project\SettingFile\Settings.xml");

                //Initializes all the required variables to Empty
                string username = string.Empty;
                string password = string.Empty;
                string domain = string.Empty;
                string mailbox = string.Empty;

                //Checks for username from excel file node and saves into string
                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("username")[0].InnerText))
                    username = reader.GetElementsByTagName("username")[0].InnerText;

                //Checks for password from excel file node and saves into string
                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("password")[0].InnerText))
                    password = reader.GetElementsByTagName("password")[0].InnerText;

                //Checks for mailboxserver from excel file node and saves into string
                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("mailboxserver")[0].InnerText))
                    mailbox = reader.GetElementsByTagName("mailboxserver")[0].InnerText;

                //Checks for domain from excel file node and saves into string
                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("domain")[0].InnerText))
                    domain = reader.GetElementsByTagName("domain")[0].InnerText;

                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("searchcondition")[0].InnerText))
                    _mailSearchCondition = reader.GetElementsByTagName("searchcondition")[0].InnerText;

                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("server")[0].InnerText))
                    _serverType = reader.GetElementsByTagName("server")[0].InnerText;

                if (!string.IsNullOrEmpty(reader.GetElementsByTagName("phrase")[0].InnerText))
                    _phraseToSearch = reader.GetElementsByTagName("phrase")[0].InnerText;



                // Set value of Mailserver details
                mailServerDetail.Username = username;
                mailServerDetail.Password = password;
                mailServerDetail.MailboxServer = mailbox;
                mailServerDetail.Domain = domain;
                GetMailServerDetails = mailServerDetail;



                Log.FileLog(Common.LogType.Success, "Mail Server details set successfully");

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
        }

        /// <summary>
        /// Downloads the attachment from the mail
        /// </summary>
        /// <param name="serverType">Server from where attachment is to be downloaded is passed as a parameter</param>
        public void DownloadAttachments()
        {
            try
            {
                //From Exchange server  (EWS)
                if (_serverType == Common.ServerType.ExchangeServer.ToString())
                    GetAttachmentsFromExchangeServer();
                else if (_serverType == Common.ServerType.Outlook.ToString()) { }
                // From Outlook (Using outlook Interop)

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }


        }

        /// <summary>
        /// Method to get attachment from exchange server
        /// </summary>
        private void GetAttachmentsFromExchangeServer()
        {
            MailSearchController mailSearchController = new MailSearchController();
            mailSearchController.PhraseToSearch = _phraseToSearch;
            try
            {
                //Creates an instance of ExchangeService
                ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

                //Provides the credentials to the Exchange Server for Login
                exchangeService.Credentials = new WebCredentials(GetMailServerDetails.Username, GetMailServerDetails.Password,
                    GetMailServerDetails.Domain);

                //Provides the mailbox URL to the server
                exchangeService.Url = new Uri(GetMailServerDetails.MailboxServer);

                //Class Mail search criteria is called and the returned value is stored in a variable
                FindItemsResults<Item> findResults = mailSearchController.MailSearchCriteria(_mailSearchCondition, exchangeService);

                //Loops for all mails in the variable 
                foreach (Item item in findResults)
                {

                    EmailMessage msg = EmailMessage.Bind(exchangeService, item.Id);

                    //Checks for attachment in the email
                    if (msg.HasAttachments)
                    {
                        //Attachments are stored in a collection
                        AttachmentCollection attachmentCollection = msg.Attachments;

                        //Loops for each attachment in the collection
                        foreach (Attachment attachment in attachmentCollection)
                        {
                            //The extensions extracted from the attachment name and stored in a variable 
                            var extension = attachment.Name.Split('.')[1];

                            //Checks the extension type
                            // Check dynamically
                            if (extension == "pdf" || extension == "txt" || extension == "jpg" || 
                                extension == "jpeg" || extension == "PNG" || extension == "png")
                            {
                                //Stores the attachment in a variable
                                FileAttachment fileAttachment = attachment as FileAttachment;
                                if (_saveAttachmentPath != string.Empty)
                                    //Attachment is saved in filepath
                                    fileAttachment.Load(_saveAttachmentPath + attachment.Name);
                                else
                                    Log.FileLog(Common.LogType.ApplicationError, "Attachment download path not set");
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

        /// <summary>
        /// This method gets attachments from Outlook
        /// </summary>
        private void GetAttachmentsFromOutlook()
        {
            try
            {

                //Uses the GetApplicationObject method to login into Outlook
                GetApplicationObject();

                //Creates an instance of Outlook
                Outlook.Application application = new Outlook.Application();

                //Gets the root folder of Outlook
                Outlook.Folder selectedFolder = application.Session.DefaultStore.GetRootFolder() as Outlook.Folder;

                //Class to Look for Inbox folder in mailbox root folder
                EnumerateFolders(selectedFolder);

                //Class to iterate through each message
                IterateMessages(selectedFolder);

                //Closes the Outlook
                Marshal.ReleaseComObject(application);
            }
            catch (Exception ex)
            {

                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            /// <summary>
            /// This class contains the method to login into Outlook
            /// </summary>
            /// 
        }

        /// <summary>
        /// Creating object to get application.
        /// This object checks and obtains any Outlook process running on the local computer.
        /// </summary>
        /// <returns></returns>
        private Outlook.Application GetApplicationObject()
        {
            //Initializes the Outlook application to null.
            Outlook.Application application = null;
            try
            {
                // Check whether there is an Outlook process running.
                if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
                {
                    // If so, use the GetActiveObject method to obtain the process and cast it to an Application object.
                    application = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
                }
                else
                {
                    // If not, create a new instance of Outlook and log on to the default profile
                    application = new Outlook.Application();
                    //Gets the MAPI namespace
                    Outlook.NameSpace nameSpace = application.GetNamespace("MAPI");
                    //Login into the outlook
                    nameSpace.Logon(GetMailServerDetails.Username, GetMailServerDetails.Password, Missing.Value, Missing.Value);
                    nameSpace = null;
                }

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            // Return the Outlook Application object.
            return application;
        }//GetAttachmentsFromOutlook


        /// <summary>
        /// This class looks into the root folder for Inbox
        /// </summary>
        /// <param name="folder">Root folder is passed</param>
        private void EnumerateFolders(Outlook.Folder folder)
        {
            try
            {
                Outlook.Folders childFolders = folder.Folders;

                if (childFolders.Count > 0)
                {
                    // loop through each childFolder (aka sub-folder) in current folder
                    foreach (Outlook.Folder childFolder in childFolders)
                    {
                        // We only want Inbox folders - ignore Contacts and others
                        if (childFolder.FolderPath.Contains("Inbox"))
                        {
                            // Call EnumerateFolders using childFolder, to see if there are any sub-folders within this one
                            EnumerateFolders(childFolder);
                        }
                    }
                }
                // pass folder to IterateMessages which processes individual email messages
                IterateMessages(folder);
            }
            catch (Exception ex)
            {
                //file log method call
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
        }//EnumerateFolders

        /// <summary>
        /// This class contains code to iterate through each message and download the attachment
        /// </summary>
        /// <param name="folder">Inbox is passed</param>
        private void IterateMessages(Outlook.Folder folder)
        {
            try
            {
                //string mailsearch = "saswat";
                //outlook.Search searchResult = OutlookMailSearch(, "saswat");
                //The items of the passed folder are stored in a variable
                var fi = folder.Items;

                //Attachment extensions to save are stored in an array
                string[] extensionsArray = { ".jpg", ".jpeg", ".pdf", ".png", ".PNG", ".txt" };

                //This block contains the code to download the attachments
                //Checks condition for folder items to be null
                if (fi != null)
                {
                    //Loops through each element(mail) in the folder
                    foreach (object item in fi)
                    {
                        //emails are stored in a variable
                        Outlook.MailItem mi = (Outlook.MailItem)item;

                        //Attachments are stored in a variable
                        var attachments = mi.Attachments;

                        //Checks whether attachment count is not zero
                        if (attachments.Count != 0)
                        {
                            //Loops through each attachment of the mail
                            for (int i = 1; i < attachments.Count; i++)
                            {
                                //Checks for the extension in the attachments
                                if (extensionsArray.Any(attachments[i].FileName.Contains))
                                {
                                    //Saves the attachment into the filepath
                                    attachments[i].SaveAsFile(_saveAttachmentPath + attachments[i].FileName);
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
        }//IterateMessages

    }//MailServerDetailController
}