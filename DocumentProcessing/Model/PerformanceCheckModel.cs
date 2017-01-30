using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Model
{
    /// <summary>
    /// Model Layer class for interacting with database to get or store performance related data
    /// </summary>
    class PerformanceCheckModel
    {


        // Global variable
        SqlConnection _sqlConnection = null;
        /// <summary>
        /// Constructor for setting sqlconnection
        /// </summary>
        /// <param name="sqlConnection">Database connection to store performance related data</param>
        public PerformanceCheckModel(SqlConnection sqlConnection)
        {
            // Sets global variable
            _sqlConnection = sqlConnection;

        }//PerformanceCheckModel

        /// <summary>
        /// Method use to store performance related data of a OCR
        /// </summary>
        /// <param name="performanceCheck"></param>
        /// <returns>bool (Data is stored successfully or not)</returns>
        public bool StoreOcrPerformance(PerformanceCheck performanceCheck)
        {
            // Uses _sqlConnection connection for storing performance related data
            //Implementation pending
            return false;

        }//StoreOcrPerformance

    }

    }
