using DocumentProcessing.Model;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Controller
{
    public class PerformanceCheckController
    {
        // Global variable
        SqlConnection _sqlConnection = null;
        /// <summary>
        /// Constructor for setting sqlconnection
        /// </summary>
        /// <param name="sqlConnection">Database connection to store performance related data</param>
        public PerformanceCheckController(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;

        }//PerformanceCheckController

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performanceCheck"></param>
        /// <returns></returns>
        public bool StoreOcrPerformance(PerformanceCheck performanceCheck)
        {
            // Uses _sqlConnection connection for storing performance related data
            //Implementation pending
            PerformanceCheckModel performanceCheckModel = null;
            if (null != performanceCheck && performanceCheck.OCR != 0)

                performanceCheckModel
                        = new PerformanceCheckModel(_sqlConnection);
            return performanceCheckModel.StoreOcrPerformance(performanceCheck);

        }//StoreOcrPerformance

    }
}
