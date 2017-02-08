using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Utility
{

    public class SqlFactory
    {
        public Database _dbConnection = null;

        public SqlFactory()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            _dbConnection = factory.CreateDefault();
        }//SqlFactory

    }//SqlFactory
}
