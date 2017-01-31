using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// Represents the view for Mailserver details
    /// Date : 21/01/2017
    /// </summary>
    public class MailServerDetail
    {
        #region Constructor
        public MailServerDetail()
        {

        }//MailServerDetailView

        #endregion Constructor

        #region Properties
        /// <summary>
        /// Get and Sets Mail server address (imap-xxx@xxx.com or pop2-xxxx@xxx.com / Exchange Server)
        /// </summary>
        public string MailboxServer { get; set; }

        /// <summary>
        /// Port no. for mail server
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///  Secured Socker Layer required or not 
        /// </summary>
        public bool SSLRequired { get; set; }

        /// <summary>
        /// Email address of the user from where we have to fetch mails 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
        #endregion Properties

    }//MailServerDetailView
}
