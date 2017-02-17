using DocumentProcessing.Utility;
using DocumentProcessing.View;
using Microsoft.Exchange.WebServices.Data;
using Outlook = Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;
using System;
using System.Linq;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;

namespace DocumentProcessing.Controller
{
    /// <summary>
    /// To get mail server details
    /// </summary>
    public class MailServerDetailController
    {
        public MailServerDetail GetMailServerDetails { get; set; }
        private MailSearch GetMailSearchDetails { get; set; }
        private MailCriteria GetMailCriteria { get; set; }
        private string _mailSearchCondition = string.Empty;
        private string _saveAttachmentPath = string.Empty;
        private string _serverType = string.Empty;
        private string _phraseToSearch = string.Empty;
        Dictionary<string, string> dictGetMetadataDetails = null;
        private string type;
        private string format;


        /// <summary>
        /// Default Constructor
        /// </summary>
        //public MailServerDetailController()
        //{

        //}//MailServerDetailController

        /// <summary>
        /// This method controls the mail server details
        /// </summary>
        /// <param name="filePathToSaveAttachments">Path to save the attachment is passed as parameter</param>
        public MailServerDetailController(string filePathToSaveAttachments)
        {

            //Contructor will load all the details from XML file
            // Path of XML to be specified
            bool exists = System.IO.Directory.Exists(filePathToSaveAttachments);

            if (!exists)
                System.IO.Directory.CreateDirectory(filePathToSaveAttachments);

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

                //if (!string.IsNullOrEmpty(reader.GetElementsByTagName("phrase")[0].InnerText))
                //    _phraseToSearch = reader.GetElementsByTagName("phrase")[0].InnerText;



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
                else if (_serverType == Common.ServerType.Outlook.ToString())
                {
                    GetAttachmentsFromOutlook();
                }
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
            //mailSearchController.PhraseToSearch = _phraseToSearch;
            //Creates an instance of ExchangeService
            ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            try
            {
                //Provides the credentials to the Exchange Server for Login
                exchangeService.Credentials = new WebCredentials(GetMailServerDetails.Username, GetMailServerDetails.Password,
                    GetMailServerDetails.Domain);

                //Provides the mailbox URL to the server
                exchangeService.Url = new Uri(GetMailServerDetails.MailboxServer);

                //Class Mail search criteria is called and the returned value is stored in a variable
                FindItemsResults<Item> findResults = mailSearchController.MailSearchCriteria2(_mailSearchCondition, exchangeService);

                MetadataController metadataController = new MetadataController();
                List<Metadata> metadataList = metadataController.GetAllMetadataDetails();
                //Loops for all mails in the variable 
                foreach (Item item in findResults)
                {
                    foreach (Metadata metadata in metadataList)
                    {
                        type = metadata.Type;
                        format = metadata.Format;

                        EmailMessage message = EmailMessage.Bind(exchangeService, item.Id);

                        //Checks for attachment in the email
                        if (message.HasAttachments)
                        {
                            //Attachments are stored in a collection
                            AttachmentCollection attachmentCollection = message.Attachments;

                            //Loops for each attachment in the collection
                            foreach (Attachment attachment in attachmentCollection)
                            {
                                string fileName = attachment.Name;
                                //The extensions extracted from the attachment name and stored in a variable
                                var fileExtension = attachment.Name.Split('.')[1];
                                if (fileName.ToLower().Contains(type.ToLower()))
                                {
                                    string supportedFormats = metadataList.Find(a => a.Type.ToLower() == type.ToLower()).Format;
                                    string[] arrayExtensions = supportedFormats.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    //Checks the extension type
                                    // Check dynamically
                                    //if (extension == format[0] || extension == format[1] || extension == format[2] ||
                                    //    extension == format[3] || extension == format[4] || extension == format[5])
                                    arrayExtensions = arrayExtensions.Select(a => a.Replace('.', ' ').Trim()).ToArray();
                                    if (arrayExtensions.Contains(fileExtension))
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

                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            Folder folder = Folder.Bind(exchangeService, WellKnownFolderName.SearchFolders);
            folder.Empty(DeleteMode.HardDelete, true);
        }

        /// <summary>
        /// This method gets attachments from Outlook
        /// </summary>
        private void GetAttachmentsFromOutlook()
        {
            try
            {
                MailSearchController mailSearchController = new MailSearchController();
                //mailSearchController.PhraseToSearch = _phraseToSearch;

                //Uses the GetApplicationObject method to login into Outlook
                GetApplicationObject();

                //Creates an instance of Outlook
                Outlook.Application application = new Outlook.Application();

                //Gets the root folder of Outlook
                Outlook.Folder selectedFolder = application.Session.DefaultStore.GetRootFolder() as Outlook.Folder;


                mailSearchController.OutlookMailSearch(application, _saveAttachmentPath);


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


    }//MailServerDetailController
}