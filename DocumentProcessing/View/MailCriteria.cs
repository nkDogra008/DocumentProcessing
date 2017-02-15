using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.View
{
    /// <summary>
    /// To get or set Mail filter Criteria
    /// </summary>
    public class MailCriteria
    {
        /// <summary>
        /// Gets and sets unique Id for each mail filter criteria 
        /// </summary>
        public int CriteriaId { get; set; }

        /// <summary>
        /// Gets and sets unique Id that defines what to filter
        /// </summary>
        public int MailSearchId { get; set; }

        /// <summary>
        /// Gets and sets what value to search for depending upon the criteria
        /// </summary>
        public string Criteria { get; set; }
    }//MailCriteria
}
