using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Utility
{
    /// <summary>
    /// SqlFactory to utilize configured and connected Database
    /// </summary>
    public class SqlFactory
    {
        public Database _dbConnection = null;

        /// <summary>
        /// SqlFactory Default Constructor
        /// </summary>
        public SqlFactory()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            _dbConnection = factory.CreateDefault();
        }//SqlFactory
    }//SqlFactory
}
