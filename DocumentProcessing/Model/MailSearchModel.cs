﻿using DocumentProcessing.Utility;
using DocumentProcessing.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Model
{
    /// <summary>
    /// 
    /// </summary>
    class MailSearchModel
    {
        public MailSearchModel()
        {

        }//MailSearchModel

        public List<MailSearch> GetAllMailSearchDetails()
        {
            List<MailSearch> listMailSearch = new List<MailSearch>();

            try
            {

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error,ex.ToString());
            }

            return listMailSearch;

        }

    }//MailSearchModel
}//MailSearchModel
