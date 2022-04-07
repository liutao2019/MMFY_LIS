using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace dcl.client.result.PatControl
{
    internal class ResultClipboard
    {
        public static DataTable DataStruct
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add("res_itm_id");
                table.Columns.Add("res_itm_ecd");
                //table.Columns.Add("res_itm_name");
                table.Columns.Add("res_chr");
                table.Columns.Add("res_od_chr");
                return table;
            }
        }


        #region singleton

        private static object padlock = new object();

        private static ResultClipboard _instance = null;

        public static ResultClipboard Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ResultClipboard();
                        }
                    }
                }
                return _instance;
            }
        }


        private ResultClipboard()
        {

        }
        #endregion

        public string itr_id { get; set; }
        public string pat_id { get; set; }
        public string pat_sid { get; set; }
        public DateTime? pat_date { get; set; }

        public DataTable resulto = null;


        public void Clear()
        {
            itr_id = null;
            pat_id = null;
            pat_sid = null;
            pat_date = null;
            resulto = null;
        }
    }
}
