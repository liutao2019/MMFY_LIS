using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.root.com;
using System.Data;
using System.Collections;
using lis.dto;
using dcl.root.dto;
//using lib.client.common;

namespace dcl.svr.sample
{
    public class BCMonitorBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        public static DataTable dtBarcode;

        public static DateTime ReadTime;

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {

            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doOther(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doSearch(DataSet ds)
        {
            ReadTime = DateTime.Now;
            DataSet dsResults = new DataSet();
            if (dtBarcode != null)
                dsResults.Tables.Add(dtBarcode.Copy());
            return dsResults;
        }

        public DataSet doUpdate(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}





