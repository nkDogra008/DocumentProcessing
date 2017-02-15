using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// To get or set Mail search filters
    /// </summary>
    public class MailSearch
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MailSearch()
        {
        }//MailSearch

        /// <summary>
        /// Gets and sets unique id for each search pattern
        /// </summary>
        public int MailSearchId { get; set; }

        /// <summary>
        /// Gets and sets subject filter
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets and sets Phrase filter
        /// </summary>
        public string Phrase { get; set; }

    }//MailSearch
}
